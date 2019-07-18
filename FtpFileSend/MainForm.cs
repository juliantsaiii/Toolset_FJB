using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpFileSend
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            labDate.Text = "启动时间为：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string intervalStr = txtInterval.Text;
            // 时间间隔 单位 分钟
            if (string.IsNullOrWhiteSpace(intervalStr))
            {
                MessageBox.Show("请输入时间间隔");
                return;
            }
            int interval = Convert.ToInt32(txtInterval.Text);
            DateTime beginDate = Convert.ToDateTime(dtpBegin.Text);
            DateTime endDate = Convert.ToDateTime(dtpEnd.Text);
            if (beginDate.AddMinutes(interval) > endDate)
            {
                MessageBox.Show("间隔时间设置过长，请重新设置间隔时间或修改时间范围");
                return;
            }
            btnOK.Enabled = false;
            ConfigHelper.ReadConfig();

            Task.Run(() =>
            {
                while (true)
                {
                    DateTime now = DateTime.Now;
                    beginDate = Convert.ToDateTime(now.ToString("yyyy-MM-dd") + " " + beginDate.ToString("HH:mm:ss"));
                    endDate = Convert.ToDateTime(now.ToString("yyyy-MM-dd") + " " + endDate.ToString("HH:mm:ss"));
                    // 运行的时间段是在开始时间和结束时间之间
                    if (now >= beginDate && now <= endDate)
                    {
                        WorkFlowTaskDAL.ExportExcel();
                    }
                    Thread.Sleep(interval * 60 * 1000);
                }
            });


        }

        /// <summary>
        /// 只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }
    }
}
