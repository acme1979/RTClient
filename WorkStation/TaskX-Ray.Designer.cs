namespace WorkStation
{
    partial class TaskX_Ray
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbSN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.tbReadRate = new System.Windows.Forms.TextBox();
            this.tbReadPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLink = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSharePassWord = new System.Windows.Forms.TextBox();
            this.tbShareUserName = new System.Windows.Forms.TextBox();
            this.tbSharePath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbSN);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1450, 47);
            this.panel1.TabIndex = 0;
            // 
            // tbSN
            // 
            this.tbSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSN.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbSN.Location = new System.Drawing.Point(162, 0);
            this.tbSN.Name = "tbSN";
            this.tbSN.Size = new System.Drawing.Size(1288, 46);
            this.tbSN.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 47);
            this.label1.TabIndex = 0;
            this.label1.Text = "刷入条码：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.tbReadRate);
            this.panel2.Controls.Add(this.tbReadPath);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.btnLink);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tbSharePassWord);
            this.panel2.Controls.Add(this.tbShareUserName);
            this.panel2.Controls.Add(this.tbSharePath);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1450, 152);
            this.panel2.TabIndex = 233;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(827, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 20);
            this.label9.TabIndex = 244;
            this.label9.Text = "秒触发上传";
            // 
            // tbReadRate
            // 
            this.tbReadRate.Location = new System.Drawing.Point(768, 101);
            this.tbReadRate.Name = "tbReadRate";
            this.tbReadRate.Size = new System.Drawing.Size(53, 30);
            this.tbReadRate.TabIndex = 243;
            this.tbReadRate.Text = "3";
            this.tbReadRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbReadPath
            // 
            this.tbReadPath.Location = new System.Drawing.Point(162, 8);
            this.tbReadPath.Name = "tbReadPath";
            this.tbReadPath.ReadOnly = true;
            this.tbReadPath.Size = new System.Drawing.Size(556, 30);
            this.tbReadPath.TabIndex = 242;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 241;
            this.label5.Text = "读取路径";
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(768, 8);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(183, 62);
            this.btnLink.TabIndex = 240;
            this.btnLink.Text = "测试链接";
            this.btnLink.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(421, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 20);
            this.label4.TabIndex = 239;
            this.label4.Text = "密码";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 20);
            this.label3.TabIndex = 238;
            this.label3.Text = "账号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 237;
            this.label2.Text = "共享路径";
            // 
            // tbSharePassWord
            // 
            this.tbSharePassWord.Location = new System.Drawing.Point(501, 101);
            this.tbSharePassWord.Name = "tbSharePassWord";
            this.tbSharePassWord.ReadOnly = true;
            this.tbSharePassWord.Size = new System.Drawing.Size(217, 30);
            this.tbSharePassWord.TabIndex = 236;
            this.tbSharePassWord.Visible = false;
            // 
            // tbShareUserName
            // 
            this.tbShareUserName.Location = new System.Drawing.Point(162, 101);
            this.tbShareUserName.Name = "tbShareUserName";
            this.tbShareUserName.ReadOnly = true;
            this.tbShareUserName.Size = new System.Drawing.Size(219, 30);
            this.tbShareUserName.TabIndex = 235;
            // 
            // tbSharePath
            // 
            this.tbSharePath.Location = new System.Drawing.Point(162, 53);
            this.tbSharePath.Name = "tbSharePath";
            this.tbSharePath.ReadOnly = true;
            this.tbSharePath.Size = new System.Drawing.Size(556, 30);
            this.tbSharePath.TabIndex = 234;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1056, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 39);
            this.button1.TabIndex = 233;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // listBoxMsg
            // 
            this.listBoxMsg.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMsg.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.ItemHeight = 19;
            this.listBoxMsg.Location = new System.Drawing.Point(0, 199);
            this.listBoxMsg.Name = "listBoxMsg";
            this.listBoxMsg.Size = new System.Drawing.Size(1450, 425);
            this.listBoxMsg.TabIndex = 234;
            // 
            // TaskX_Ray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxMsg);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "TaskX_Ray";
            this.Size = new System.Drawing.Size(1450, 624);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSN;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbReadRate;
        private System.Windows.Forms.TextBox tbReadPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSharePassWord;
        private System.Windows.Forms.TextBox tbShareUserName;
        private System.Windows.Forms.TextBox tbSharePath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBoxMsg;

    }
}
