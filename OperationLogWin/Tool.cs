using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace OperationLogWin
{
    public class Tool
    {

        /// <summary>
        /// 操作类型
        /// </summary>
        public static List<string> operationTypeList = new List<string> { "删除","新增","修改","查询"};

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public static List<UserModel> GetAllUser()
        {
            string sql = "SELECT ID,`Name`,FullName,CompanyID,CompanyName from `user`";
            DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            return TableToList.ToDataList<UserModel>(dt);
        }

        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="listUser"></param>
        /// <param name="listAllTables"></param>
        /// <returns></returns>
        public static bool InsertLog(List<UserModel> listUser, List<TableModel> listAllTables)
        {
            Random rd = new Random();
            int index = rd.Next(0, listUser.Count);
            UserModel user = listUser[index];

            Random rdOp = new Random();
            int rdopIndex = rdOp.Next(0, operationTypeList.Count);
            string operationType = operationTypeList[rdopIndex];

            Random rdTable = new Random();
            int rdTableIndex = rdTable.Next(0, listAllTables.Count);
            string tableName = listAllTables[rdTableIndex].table_name;

            string insertSql = $@"INSERT INTO `operationlog` (`ID`,`UserID`,`LoginName`,
                                 `FullName`,`CompanyID`,`CompanyName`,`TableName`,`OperationType`,
                                 `CreateTime`)
                                VALUES('{Guid.NewGuid().ToString()}',
		'{user.ID}',
		'{user.Name}',
		'{user.FullName}',
		'{user.CompanyID}',
		'{user.CompanyName}',
		'{tableName}',
		'{operationType}',
		'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'
	);
";
          return  MySQLHelper.ExecuteNonQuery(CommandType.Text, insertSql)>0;
        }

        /// <summary>
        /// 获取数据库中指定的表
        /// </summary>
        /// <returns></returns>
        public static List<TableModel> GetAllTables()
        {
            string sql = $@"select table_name from information_schema.tables where 
                            table_schema = '{ConfigurationManager.AppSettings["DataTableName"]}' and table_type = 'base table'";
            DataTable dt = MySQLHelper.GetDataTable(CommandType.Text, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<TableModel> listTable = TableToList.ToDataList<TableModel>(dt);
           return listTable.FindAll(o=>o.table_name.ToLower().Contains("temp_") || o.table_name.ToLower().Contains("mea_")).ToList();
        }
    }
}
