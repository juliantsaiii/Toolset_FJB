using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperationLogWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            btnOK.Text = "生成中";
            btnOK.Enabled = false;
            try
            {
                List<UserModel> listUser = Tool.GetAllUser();
                List<TableModel> listTable = Tool.GetAllTables();
                string count = this.txtCount.Text;
                if (string.IsNullOrEmpty(count))
                {
                    Tool.InsertLog(listUser, listTable);
                }

                else
                {
                    for (int i = 0; i < Convert.ToInt32(count); i++)
                    {
                        Task.Run(() => {
                            Tool.InsertLog(listUser, listTable);
                        });
                    }
                }
                btnOK.Enabled = true;
                btnOK.Text = "生成";
                MessageBox.Show("生成成功");
            }
            catch (Exception ex)
            {

              
                btnOK.Enabled = true;
                btnOK.Text = "生成";
                MessageBox.Show(ex.Message);
            }
            
           
           
        }
    }
}
