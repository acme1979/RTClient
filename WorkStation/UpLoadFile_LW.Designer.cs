namespace WorkStation
{
    partial class UpLoadFile_LW
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckRecursion = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbIntervalHour = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbmFileType = new System.Windows.Forms.ComboBox();
            this.ckMsg = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbReadRate = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblStateTitle = new System.Windows.Forms.Label();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbUploadPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbBackPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbReadPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckRecursion);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.tbIntervalHour);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbmFileType);
            this.panel1.Controls.Add(this.ckMsg);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.tbReadRate);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.lblState);
            this.panel1.Controls.Add(this.lblStateTitle);
            this.panel1.Controls.Add(this.btnEnd);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.tbUploadPath);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tbBackPath);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbReadPath);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1355, 269);
            this.panel1.TabIndex = 13;
            // 
            // ckRecursion
            // 
            this.ckRecursion.AutoSize = true;
            this.ckRecursion.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckRecursion.ForeColor = System.Drawing.Color.Red;
            this.ckRecursion.Location = new System.Drawing.Point(886, 168);
            this.ckRecursion.Name = "ckRecursion";
            this.ckRecursion.Size = new System.Drawing.Size(171, 24);
            this.ckRecursion.TabIndex = 227;
            this.ckRecursion.Text = "是否递归子目录";
            this.ckRecursion.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(909, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 20);
            this.label6.TabIndex = 226;
            this.label6.Text = "小时之前的文件";
            // 
            // tbIntervalHour
            // 
            this.tbIntervalHour.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbIntervalHour.Location = new System.Drawing.Point(859, 124);
            this.tbIntervalHour.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbIntervalHour.Name = "tbIntervalHour";
            this.tbIntervalHour.Size = new System.Drawing.Size(44, 30);
            this.tbIntervalHour.TabIndex = 225;
            this.tbIntervalHour.Text = "10";
            this.tbIntervalHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(804, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 20);
            this.label5.TabIndex = 224;
            this.label5.Text = "上传";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(804, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 223;
            this.label4.Text = "文件类型";
            // 
            // cbmFileType
            // 
            this.cbmFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbmFileType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbmFileType.FormattingEnabled = true;
            this.cbmFileType.Location = new System.Drawing.Point(900, 70);
            this.cbmFileType.Name = "cbmFileType";
            this.cbmFileType.Size = new System.Drawing.Size(217, 28);
            this.cbmFileType.TabIndex = 222;
            // 
            // ckMsg
            // 
            this.ckMsg.AutoSize = true;
            this.ckMsg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckMsg.Location = new System.Drawing.Point(857, 220);
            this.ckMsg.Name = "ckMsg";
            this.ckMsg.Size = new System.Drawing.Size(111, 24);
            this.ckMsg.TabIndex = 221;
            this.ckMsg.Text = "失败弹窗";
            this.ckMsg.UseVisualStyleBackColor = true;
            this.ckMsg.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(1028, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 25);
            this.label13.TabIndex = 220;
            this.label13.Text = "s";
            // 
            // tbReadRate
            // 
            this.tbReadRate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbReadRate.Location = new System.Drawing.Point(900, 26);
            this.tbReadRate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbReadRate.Name = "tbReadRate";
            this.tbReadRate.Size = new System.Drawing.Size(120, 30);
            this.tbReadRate.TabIndex = 219;
            this.tbReadRate.Text = "10";
            this.tbReadRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(805, 31);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 20);
            this.label12.TabIndex = 218;
            this.label12.Text = "读取频率";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblState.ForeColor = System.Drawing.Color.Red;
            this.lblState.Location = new System.Drawing.Point(177, 179);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(0, 27);
            this.lblState.TabIndex = 217;
            // 
            // lblStateTitle
            // 
            this.lblStateTitle.AutoSize = true;
            this.lblStateTitle.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStateTitle.ForeColor = System.Drawing.Color.Red;
            this.lblStateTitle.Location = new System.Drawing.Point(21, 179);
            this.lblStateTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStateTitle.Name = "lblStateTitle";
            this.lblStateTitle.Size = new System.Drawing.Size(147, 27);
            this.lblStateTitle.TabIndex = 216;
            this.lblStateTitle.Text = "读取状态：";
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.Color.Red;
            this.btnEnd.Enabled = false;
            this.btnEnd.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnEnd.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEnd.Location = new System.Drawing.Point(1155, 98);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(131, 75);
            this.btnEnd.TabIndex = 22;
            this.btnEnd.Text = "停止\r\n运行";
            this.btnEnd.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(1155, 13);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(131, 75);
            this.btnStart.TabIndex = 21;
            this.btnStart.Text = "启动\r\n运行";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1155, 201);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 62);
            this.button1.TabIndex = 20;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // tbUploadPath
            // 
            this.tbUploadPath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbUploadPath.Location = new System.Drawing.Point(167, 126);
            this.tbUploadPath.Name = "tbUploadPath";
            this.tbUploadPath.ReadOnly = true;
            this.tbUploadPath.Size = new System.Drawing.Size(605, 30);
            this.tbUploadPath.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(70, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "上传目录";
            // 
            // tbBackPath
            // 
            this.tbBackPath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbBackPath.Location = new System.Drawing.Point(167, 75);
            this.tbBackPath.Name = "tbBackPath";
            this.tbBackPath.ReadOnly = true;
            this.tbBackPath.Size = new System.Drawing.Size(605, 30);
            this.tbBackPath.TabIndex = 17;
            this.tbBackPath.Text = "D:\\Backup\\桌面\\炉温\\back";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(70, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "备份目录";
            // 
            // tbReadPath
            // 
            this.tbReadPath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbReadPath.Location = new System.Drawing.Point(167, 26);
            this.tbReadPath.Name = "tbReadPath";
            this.tbReadPath.ReadOnly = true;
            this.tbReadPath.Size = new System.Drawing.Size(605, 30);
            this.tbReadPath.TabIndex = 15;
            this.tbReadPath.Text = "D:\\Backup\\桌面\\炉温\\Upload\\lw";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(70, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "读取目录";
            // 
            // listBoxMsg
            // 
            this.listBoxMsg.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMsg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.ItemHeight = 20;
            this.listBoxMsg.Location = new System.Drawing.Point(0, 269);
            this.listBoxMsg.Name = "listBoxMsg";
            this.listBoxMsg.Size = new System.Drawing.Size(1355, 266);
            this.listBoxMsg.TabIndex = 14;
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            // 
            // UpLoadFile_LW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxMsg);
            this.Controls.Add(this.panel1);
            this.Name = "UpLoadFile_LW";
            this.Size = new System.Drawing.Size(1355, 535);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbUploadPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbBackPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbReadPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbReadRate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblStateTitle;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.CheckBox ckMsg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbmFileType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbIntervalHour;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckRecursion;

    }
}
