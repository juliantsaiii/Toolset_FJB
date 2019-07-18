using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationLogWin
{
    public class UserModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
    }

    public class TableModel
    {
        public string table_name { get; set; }
    }
 
}
