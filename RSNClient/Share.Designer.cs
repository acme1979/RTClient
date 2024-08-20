namespace RTClient
{
    partial class Share
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label9 = new System.Windows.Forms.Label();
            this.tbReadRate = new System.Windows.Forms.TextBox();
            this.tbReadPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLink = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbShareUserName = new System.Windows.Forms.TextBox();
            this.tbSharePath = new System.Windows.Forms.TextBox();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.tbSharePassWord = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblState = new System.Windows.Forms.Label();
            this.lblStateTitle = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbBackPath = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(330, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 15);
            this.label9.TabIndex = 244;
            this.label9.Text = "秒触发上传";
            // 
            // tbReadRate
            // 
            this.tbReadRate.Location = new System.Drawing.Point(271, 130);
            this.tbReadRate.Name = "tbReadRate";
            this.tbReadRate.Size = new System.Drawing.Size(53, 25);
            this.tbReadRate.TabIndex = 243;
            this.tbReadRate.Text = "3";
            this.tbReadRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbReadPath
            // 
            this.tbReadPath.Location = new System.Drawing.Point(86, 25);
            this.tbReadPath.Name = "tbReadPath";
            this.tbReadPath.ReadOnly = true;
            this.tbReadPath.Size = new System.Drawing.Size(350, 25);
            this.tbReadPath.TabIndex = 242;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 241;
            this.label5.Text = "读取路径";
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(86, 180);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(144, 38);
            this.btnLink.TabIndex = 240;
            this.btnLink.Text = "测试链接";
            this.btnLink.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 238;
            this.label3.Text = "账号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 237;
            this.label2.Text = "共享路径";
            // 
            // tbShareUserName
            // 
            this.tbShareUserName.Location = new System.Drawing.Point(86, 130);
            this.tbShareUserName.Name = "tbShareUserName";
            this.tbShareUserName.ReadOnly = true;
            this.tbShareUserName.Size = new System.Drawing.Size(146, 25);
            this.tbShareUserName.TabIndex = 235;
            // 
            // tbSharePath
            // 
            this.tbSharePath.Location = new System.Drawing.Point(86, 93);
            this.tbSharePath.Name = "tbSharePath";
            this.tbSharePath.ReadOnly = true;
            this.tbSharePath.Size = new System.Drawing.Size(350, 25);
            this.tbSharePath.TabIndex = 234;
            // 
            // listBoxMsg
            // 
            this.listBoxMsg.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMsg.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.ItemHeight = 23;
            this.listBoxMsg.Location = new System.Drawing.Point(0, 0);
            this.listBoxMsg.Name = "listBoxMsg";
            this.listBoxMsg.Size = new System.Drawing.Size(505, 515);
            this.listBoxMsg.TabIndex = 235;
            // 
            // tbSharePassWord
            // 
            this.tbSharePassWord.Location = new System.Drawing.Point(109, 406);
            this.tbSharePassWord.Name = "tbSharePassWord";
            this.tbSharePassWord.ReadOnly = true;
            this.tbSharePassWord.Size = new System.Drawing.Size(74, 25);
            this.tbSharePassWord.TabIndex = 236;
            this.tbSharePassWord.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(66, 415);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 239;
            this.label4.Text = "密码";
            this.label4.Visible = false;
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.Color.Red;
            this.btnEnd.Enabled = false;
            this.btnEnd.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnEnd.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEnd.Location = new System.Drawing.Point(248, 240);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(131, 75);
            this.btnEnd.TabIndex = 246;
            this.btnEnd.Text = "停止\r\n运行";
            this.btnEnd.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(86, 237);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(146, 78);
            this.btnStart.TabIndex = 245;
            this.btnStart.Text = "启动\r\n运行";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblState.ForeColor = System.Drawing.Color.Red;
            this.lblState.Location = new System.Drawing.Point(105, 341);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(0, 20);
            this.lblState.TabIndex = 248;
            // 
            // lblStateTitle
            // 
            this.lblStateTitle.AutoSize = true;
            this.lblStateTitle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStateTitle.ForeColor = System.Drawing.Color.Red;
            this.lblStateTitle.Location = new System.Drawing.Point(25, 341);
            this.lblStateTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStateTitle.Name = "lblStateTitle";
            this.lblStateTitle.Size = new System.Drawing.Size(69, 20);
            this.lblStateTitle.TabIndex = 247;
            this.lblStateTitle.Text = "状态：";
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbBackPath);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.lblState);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnLink);
            this.panel1.Controls.Add(this.lblStateTitle);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnEnd);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbSharePath);
            this.panel1.Controls.Add(this.tbReadPath);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.tbSharePassWord);
            this.panel1.Controls.Add(this.tbShareUserName);
            this.panel1.Controls.Add(this.tbReadRate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(449, 515);
            this.panel1.TabIndex = 249;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 249;
            this.label1.Text = "备份路径";
            // 
            // tbBackPath
            // 
            this.tbBackPath.Location = new System.Drawing.Point(86, 59);
            this.tbBackPath.Name = "tbBackPath";
            this.tbBackPath.ReadOnly = true;
            this.tbBackPath.Size = new System.Drawing.Size(350, 25);
            this.tbBackPath.TabIndex = 250;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBoxMsg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(449, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(505, 515);
            this.panel2.TabIndex = 250;
            // 
            // Share
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 515);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Share";
            this.Text = "共享文件上传";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbReadRate;
        private System.Windows.Forms.TextBox tbReadPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbShareUserName;
        private System.Windows.Forms.TextBox tbSharePath;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSharePassWord;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblStateTitle;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbBackPath;
        private System.Windows.Forms.Panel panel2;
    }
}