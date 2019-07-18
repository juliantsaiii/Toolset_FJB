using System.Configuration;

namespace FileMigration
{
    public class ConfigHelper
    {
        #region 常量 
        /// <summary>
        /// 文书上传的附件路径
        /// </summary>
        public const string WenShuPath = "/UploadFiles/wenshu/";
        #endregion

        #region 变量
        /// <summary>
        /// 需要同步的文件所在的文件夹
        /// </summary>
        public static string DirectoryPath { get; private set; }
        /// <summary>
        /// 文件移动后的文件夹
        /// </summary>
        public static string RemoveFilePath { get; private set; }
        #endregion

        public static void ReadAppConfig()
        {
            ConfigurationManager.RefreshSection("appSettings");
            DirectoryPath = ConfigurationManager.AppSettings["DirectoryPath"];
            RemoveFilePath = ConfigurationManager.AppSettings["RemoveFilePath"];
        }

    }
}
