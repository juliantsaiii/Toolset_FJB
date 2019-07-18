using CommonLibrary;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FileMigration
{
    public class FileUploadDAL
    {
        /// <summary>
        /// 查找所有需要同步的文件信息
        /// </summary>
        /// <returns></returns>
        public static List<FileUploadModel> GetNeedFileMigration()
        {
            // 查找所有需要同步的文件信息
            string sql = @"SELECT a.ID AS FileUploadID,UserID,BasePath,FileExtend,FileName,b.CompanyID FROM
	                       fileupload AS a INNER JOIN USER AS b ON a.UserID = b.ID  WHERE sourceType = 1 
                           AND IsSync = 0 ORDER BY b.CompanyID";
            DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            return TableToList.ToDataList<FileUploadModel>(dt);

        }

 
        /// <summary>
        /// 更新文件状态
        /// </summary>
        /// <param name="isSync"></param>
        /// <param name="fileUploadID">文件ID</param>
        /// <returns></returns>
        public static bool UpdateFileUploadIsSync(int isSync, string fileUploadID)
        {
            string sql = string.Format("update fileupload set IsSync = {0} where ID = '{1}'", isSync, fileUploadID);
            return MySQLHelper.ExecuteNonQuery(CommandType.Text, sql) > 0;
        }

        #region 递归查询当前用户的的顶级companyID
        /// <summary>
        /// 递归查询当前用户的的顶级companyID
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static string GetParentCompanyID(string companyID,int index = 0)
        {
            string sql = string.Format("SELECT * FROM DEPT WHERE ID = '{0}'",companyID);
            DataTable dt =  MySQLHelper.GetDataTable(CommandType.Text, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            Dept dept = TableToList.ToDataList<Dept>(dt).FirstOrDefault();
            // 防止出现死循环 所以这边用index标记 最大深度10次
            if (dept != null && dept.PID != Constant.DeptParentGuid && index < 10)
            {
                index++;
                return GetParentCompanyID(dept.PID,index);
            }
            return dept.ID;
        }
        #endregion

        #region 获取所有文件服务器信息
        /// <summary>
        /// 获取所有文件服务器信息
        /// </summary>
        /// <returns></returns>
        public static List<FileServerMapping> GetVirtualDirectory()
        {
            string sql = "SELECT * FROM fileservermapping";
            DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
           return TableToList.ToDataList<FileServerMapping>(dt);
        }
        #endregion

    }

   
}
