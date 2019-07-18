using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.FileBinaryTransferSoapClient cli = new ServiceReference1.FileBinaryTransferSoapClient();
            TestWebService.ServiceReference1.ArrayOfString aaa = new ServiceReference1.ArrayOfString();
            cli.SaveFile(new byte[1], "", aaa, "", "", "", "", "");
        }
    }
}
