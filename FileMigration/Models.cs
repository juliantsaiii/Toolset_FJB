
using System;

namespace FileMigration
{
    public partial class Dept
    {
        public string ID { get; set; }
        public string PID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int SortCode { get; set; }
        public string SerialNumber { get; set; }
        public bool Deleted { get; set; }
        public string FlowInfoType { get; set; }
    }

    public class FileUploadModel
    {
        public string FileUploadID { get; set; }
        public string UserID { get; set; }
        public string BasePath { get; set; }
        public string FileExtend { get; set; }
        public string FileName { get; set; }
        public string CompanyID { get; set; }
    }

    public class FileServerMapping
    {             
        public string CompanyID { get; set; }             
        public string MapIPAddress { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
    }
}
