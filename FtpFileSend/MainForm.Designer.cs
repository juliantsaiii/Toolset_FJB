namespace FtpFileSend
{
    partial class mainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.labDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(291, 377);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(158, 50);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "启动";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Location = new System.Drawing.Point(215, 469);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(0, 24);
            this.labDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "开始时间段:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "结束时间段：";
            // 
            // dtpBegin
            // 
            this.dtpBegin.CustomFormat = "";
            this.dtpBegin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpBegin.Location = new System.Drawing.Point(335, 99);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Size = new System.Drawing.Size(235, 35);
            this.dtpBegin.TabIndex = 5;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEnd.Location = new System.Drawing.Point(335, 184);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(235, 35);
            this.dtpEnd.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(111, 275);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "发送间隔(min)：";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(335, 264);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(235, 35);
            this.txtInterval.TabIndex = 8;
            this.txtInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInterval_KeyPress);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 586);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpBegin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labDate);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Text = "FTP文件发送";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInterval;
    }
}

