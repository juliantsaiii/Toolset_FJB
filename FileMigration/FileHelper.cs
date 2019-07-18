using System;
using System.Diagnostics;
using System.IO;

namespace FileMigration
{
    public class FileHelper
    {
        #region 获取指定文件夹下的文件
        /// <summary>
        /// 获取指定文件夹下的文件
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns></returns>
        public static FileInfo[] GeFiles(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo root = new DirectoryInfo(directoryPath);
                return root.GetFiles();
            }
            else
            {
                LogHelper.DoNormalLog(string.Format("{0}文件夹不存在", directoryPath));
                return null;
            }
        }
        #endregion

        #region 连接远程共享文件夹
        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool ConnectState(string path, string userName, string passWord)
        {
            bool flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "net use " + path + " " + passWord + " /user:" + userName;
                LogHelper.DoNormalLog(string.Format("连接远程服务器的DOS命令为:【{0}】",dosLine));
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errorMsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errorMsg))
                {
                    flag = true;
                }
                else
                {
                    LogHelper.DoNormalLog(string.Format("远程服务器链接失败，原因：{0}", errorMsg));
                }
            }
            catch (Exception ex)
            {
                LogHelper.DoNormalLog(string.Format("连接远程共享文件夹失败，具体原因为：{0}", ex.Message));
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return flag;
        }
        #endregion

        #region 向远程文件夹保存本地内容
        /// <summary>
        /// 向远程文件夹保存本地内容
        /// </summary>
        /// <param name="src">要保存的文件的路径，如果保存文件到共享文件夹，这个路径就是本地文件路径如：@"D:\1.avi"</param>
        /// <param name="dst">保存文件的路径，不含名称及扩展名</param>
        /// <param name="fileName">保存文件的名称以及扩展名</param>
        public static bool Transport(string src, string dst, string fileName)
        {
            bool result = false;
            try
            {
                FileStream inFileStream = new FileStream(src, FileMode.Open);
                if (!Directory.Exists(dst))
                {
                    Directory.CreateDirectory(dst);
                }
                dst = dst + fileName;
                FileStream outFileStream = new FileStream(dst, FileMode.OpenOrCreate);

                byte[] buf = new byte[inFileStream.Length];

                int byteCount;

                while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
                {
                    outFileStream.Write(buf, 0, byteCount);
                }

                inFileStream.Flush();
                inFileStream.Close();
                outFileStream.Flush();
                outFileStream.Close();
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.DoNormalLog(string.Format("向远程文件夹保存本地文件失败，具体原因为：{0}", ex.Message));
            }
            return result;
        }
        #endregion

        #region 移动文件
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourcePath">源文件地址</param>
        /// <param name="destPath">目标文件地址 也就是把源文件需要移动到某一个地方的地址</param>
        /// <param name="fileName">移动后的文件的名称</param>
        public static bool MoveFile(string sourcePath, string destPath, string fileName)
        {
            bool result = true;
            try
            {
                if (File.Exists(sourcePath))
                {
                    if (!Directory.Exists(destPath))
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    if (File.Exists(Path.Combine(destPath, fileName)))
                    {
                        fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), fileName);
                    }
                    //参数1：要移动的源文件路径，参数2：移动后的目标文件路径
                    File.Move(sourcePath, Path.Combine(destPath, fileName));
                }
            }
            catch (Exception ex)
            {
                LogHelper.DoNormalLog(string.Format("移动文件失败，具体原因为：{0}", ex.Message));
                result = false;

            }
            return result;

        }
        #endregion
    }
}
