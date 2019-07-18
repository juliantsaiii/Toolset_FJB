using System.Configuration;

namespace FtpFileSend
{
    public class ConfigHelper
    {  
        /// <summary>
        /// 导出的Excel名称
        /// </summary>
        public static string ExcelName { get; set; }
        /// <summary>
        /// FTP服务器地址
        /// </summary>
        public static string FTPAddress { get; set; }
        /// <summary>
        /// FTP服务器密码
        /// </summary>
        public static string FTPPwd { get; set; }
        /// <summary>
        /// FTP服务器用户名
        /// </summary>
        public static string FTPName { get; set; }
        /// <summary>
        /// 查询sql
        /// </summary>
        public static string SearchSql { get; set; }
        /// <summary>
        /// 额外需要发送的信息
        /// </summary>
        public static string SendMessage { get; set; }

        /// <summary>
        /// 初始化读取配置文件信息
        /// </summary>
        public static void ReadConfig()
        {
            ExcelName = ConfigurationManager.AppSettings["ExcelName"];
            FTPAddress = ConfigurationManager.AppSettings["FTPAddress"];
            FTPPwd = ConfigurationManager.AppSettings["FTPPwd"];
            FTPName = ConfigurationManager.AppSettings["FTPName"];
            SearchSql = ConfigurationManager.AppSettings["SearchSql"];
            SendMessage = ConfigurationManager.AppSettings["SendMessage"];
        }
    }
}
