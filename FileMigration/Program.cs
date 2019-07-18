using System.Collections.Generic;
using System.IO;

namespace FileMigration
{
    class Program
    {

        static void Main(string[] args)
        {
            // 读取配置文件
            ConfigHelper.ReadAppConfig();
            LogHelper.DoNormalLog("====文件操作开始====");
            List<FileUploadModel> list = FileUploadDAL.GetNeedFileMigration();
            if (list == null)
            {
                LogHelper.DoNormalLog("此次没有需要同步的文件");
                LogHelper.DoNormalLog("====文件操作结束====");
                return;
            }

            // 获取所有文件服务器信息
            List<FileServerMapping> fileServerList = FileUploadDAL.GetVirtualDirectory();

            LogHelper.DoNormalLog(string.Format("本次一共需要移动【{0}】个文件", list.Count));
            string companyID = string.Empty;
            bool status = false;
            foreach (var item in list)
            {
                // 获取文件所在的路径
                string filePath = Path.Combine(ConfigHelper.DirectoryPath, item.BasePath, item.FileUploadID + item.FileExtend);
                LogHelper.DoNormalLog(string.Format("FileUpload表中ID为【{0}】的文件路径为【{1}】", item.FileUploadID,filePath));
                if (!File.Exists(filePath))
                {
                    //如果数据库中有数据，但是磁盘中没有该文件，则记录下当前信息，并把数据库中IsSync标记为2
                    LogHelper.DoNormalLog(string.Format("FileUpload表中ID为【{0}】的文件在磁盘中不存在", item.FileUploadID));
                    FileUploadDAL.UpdateFileUploadIsSync(2, item.FileUploadID);
                }
                else
                {
                    var parentParentCompanyID = FileUploadDAL.GetParentCompanyID(item.CompanyID);
                    var currentServer = fileServerList.Find(o => o.CompanyID == parentParentCompanyID);
                    if (companyID != parentParentCompanyID)
                    {
                        companyID = parentParentCompanyID;              
                        if (currentServer != null)
                        {
                            LogHelper.DoNormalLog(string.Format("文件服务器为【{0}】，用户名为【{1}】，密码为【{2}】", currentServer.MapIPAddress, currentServer.UserName, currentServer.UserPwd));
                            //连接共享文件夹
                            status = FileHelper.ConnectState(currentServer.MapIPAddress.Trim(), currentServer.UserName.Trim(), currentServer.UserPwd.Trim());
                        }
                    }

                    if (status)
                    {
                        //共享文件夹的目录
                        DirectoryInfo theFolder = new DirectoryInfo(currentServer.MapIPAddress);
                        //获取保存文件的路径
                        string destPath = theFolder.ToString() + ConfigHelper.WenShuPath + item.BasePath;
                        //保存到远程服务器的文件名
                        string destFileName = item.FileUploadID + item.FileExtend;
                        //向远程文件夹保存文件
                        bool transportResult = FileHelper.Transport(filePath, destPath, destFileName);
                        if (transportResult)
                        {
                            // 保存成功了就更新表状态 为1
                            FileUploadDAL.UpdateFileUploadIsSync(1, item.FileUploadID);
                            LogHelper.DoNormalLog(string.Format("文件:【{0}】复制到远程服务器成功", destFileName));
                            //移动文件
                            bool moveResult = FileHelper.MoveFile(filePath, ConfigHelper.RemoveFilePath, destFileName);
                            if (moveResult)
                            {
                                LogHelper.DoNormalLog(string.Format("文件:【{0}】移动到【{1}】成功", filePath, Path.Combine(ConfigHelper.RemoveFilePath, destFileName)));
                            }
                        }

                    }
                }
            }
            LogHelper.DoNormalLog("====文件操作结束====");
        }
    }

   
}