namespace WorkStation
{
    partial class WorkStationCtl
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
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel7 = new System.Windows.Forms.Panel();
            this.timerMusic = new System.Windows.Forms.Timer(this.components);
            this.pnlExtend = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnYC = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnDB = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.panel11 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbProductLine = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbWSName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbUName = new System.Windows.Forms.TextBox();
            this.tbUId = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(923, 0);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(7, 666);
            this.splitter2.TabIndex = 13;
            this.splitter2.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 900000;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.PaleGreen;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.panel11);
            this.panel7.Controls.Add(this.panel8);
            this.panel7.Controls.Add(this.panel1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(930, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(250, 666);
            this.panel7.TabIndex = 11;
            // 
            // timerMusic
            // 
            this.timerMusic.Interval = 400;
            // 
            // pnlExtend
            // 
            this.pnlExtend.BackColor = System.Drawing.SystemColors.Control;
            this.pnlExtend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlExtend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExtend.Location = new System.Drawing.Point(0, 0);
            this.pnlExtend.Margin = new System.Windows.Forms.Padding(4);
            this.pnlExtend.Name = "pnlExtend";
            this.pnlExtend.Size = new System.Drawing.Size(923, 666);
            this.pnlExtend.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnYC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(46, 664);
            this.panel1.TabIndex = 0;
            // 
            // btnYC
            // 
            this.btnYC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnYC.Location = new System.Drawing.Point(0, 0);
            this.btnYC.Name = "btnYC";
            this.btnYC.Size = new System.Drawing.Size(46, 664);
            this.btnYC.TabIndex = 0;
            this.btnYC.Text = "隐\r\n\r\n藏";
            this.btnYC.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel8.Controls.Add(this.btnDB);
            this.panel8.Controls.Add(this.btnSetup);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(46, 574);
            this.panel8.Margin = new System.Windows.Forms.Padding(4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(202, 90);
            this.panel8.TabIndex = 16;
            // 
            // btnDB
            // 
            this.btnDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDB.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDB.Location = new System.Drawing.Point(0, 0);
            this.btnDB.Margin = new System.Windows.Forms.Padding(4);
            this.btnDB.Name = "btnDB";
            this.btnDB.Size = new System.Drawing.Size(198, 39);
            this.btnDB.TabIndex = 11;
            this.btnDB.TabStop = false;
            this.btnDB.Text = "数据库设置";
            this.btnDB.UseVisualStyleBackColor = true;
            // 
            // btnSetup
            // 
            this.btnSetup.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSetup.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetup.Location = new System.Drawing.Point(0, 39);
            this.btnSetup.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(198, 47);
            this.btnSetup.TabIndex = 10;
            this.btnSetup.TabStop = false;
            this.btnSetup.Text = "参数设置";
            this.btnSetup.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel11.Controls.Add(this.groupBox3);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(46, 177);
            this.panel11.Margin = new System.Windows.Forms.Padding(4);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(202, 397);
            this.panel11.TabIndex = 17;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbProductLine);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tbPName);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.tbWSName);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.tbUName);
            this.groupBox3.Controls.Add(this.tbUId);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 15);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(198, 378);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "当前工站信息";
            // 
            // tbProductLine
            // 
            this.tbProductLine.BackColor = System.Drawing.Color.PaleGreen;
            this.tbProductLine.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbProductLine.ForeColor = System.Drawing.Color.Black;
            this.tbProductLine.Location = new System.Drawing.Point(7, 261);
            this.tbProductLine.Margin = new System.Windows.Forms.Padding(4);
            this.tbProductLine.Name = "tbProductLine";
            this.tbProductLine.ReadOnly = true;
            this.tbProductLine.Size = new System.Drawing.Size(296, 35);
            this.tbProductLine.TabIndex = 17;
            this.tbProductLine.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 232);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 24);
            this.label1.TabIndex = 16;
            this.label1.Text = "当前线别名称：";
            // 
            // tbPName
            // 
            this.tbPName.BackColor = System.Drawing.Color.PaleGreen;
            this.tbPName.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPName.ForeColor = System.Drawing.Color.Black;
            this.tbPName.Location = new System.Drawing.Point(7, 191);
            this.tbPName.Margin = new System.Windows.Forms.Padding(4);
            this.tbPName.Name = "tbPName";
            this.tbPName.ReadOnly = true;
            this.tbPName.Size = new System.Drawing.Size(296, 35);
            this.tbPName.TabIndex = 13;
            this.tbPName.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(7, 162);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(185, 24);
            this.label11.TabIndex = 12;
            this.label11.Text = "当前工序名称：";
            // 
            // tbWSName
            // 
            this.tbWSName.BackColor = System.Drawing.Color.PaleGreen;
            this.tbWSName.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbWSName.ForeColor = System.Drawing.Color.Black;
            this.tbWSName.Location = new System.Drawing.Point(7, 331);
            this.tbWSName.Margin = new System.Windows.Forms.Padding(4);
            this.tbWSName.Name = "tbWSName";
            this.tbWSName.ReadOnly = true;
            this.tbWSName.Size = new System.Drawing.Size(296, 35);
            this.tbWSName.TabIndex = 9;
            this.tbWSName.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(7, 302);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(185, 24);
            this.label13.TabIndex = 5;
            this.label13.Text = "工作中心名称：";
            // 
            // tbUName
            // 
            this.tbUName.BackColor = System.Drawing.Color.PaleGreen;
            this.tbUName.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbUName.ForeColor = System.Drawing.Color.Black;
            this.tbUName.Location = new System.Drawing.Point(7, 121);
            this.tbUName.Margin = new System.Windows.Forms.Padding(4);
            this.tbUName.Name = "tbUName";
            this.tbUName.ReadOnly = true;
            this.tbUName.Size = new System.Drawing.Size(296, 35);
            this.tbUName.TabIndex = 3;
            this.tbUName.TabStop = false;
            // 
            // tbUId
            // 
            this.tbUId.BackColor = System.Drawing.Color.PaleGreen;
            this.tbUId.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbUId.ForeColor = System.Drawing.Color.Black;
            this.tbUId.Location = new System.Drawing.Point(7, 51);
            this.tbUId.Margin = new System.Windows.Forms.Padding(4);
            this.tbUId.Name = "tbUId";
            this.tbUId.ReadOnly = true;
            this.tbUId.Size = new System.Drawing.Size(296, 35);
            this.tbUId.TabIndex = 2;
            this.tbUId.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(7, 92);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(185, 24);
            this.label15.TabIndex = 1;
            this.label15.Text = "操作人员姓名：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(7, 22);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(185, 24);
            this.label16.TabIndex = 0;
            this.label16.Text = "操作人员工号：";
            // 
            // WorkStationCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlExtend);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel7);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "WorkStationCtl";
            this.Size = new System.Drawing.Size(1180, 666);
            this.panel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Timer timerMusic;
        private System.Windows.Forms.DataGridViewTextBoxColumn sNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dC01DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dC02DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dC03DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA01DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn crtorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn crtDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA04DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA05DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA06DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA07DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA08DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA09DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA10DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA28DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bA29DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bA48DataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bA49DataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bA50DataGridViewCheckBoxColumn;
        private System.Windows.Forms.Panel pnlExtend;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnYC;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbProductLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbWSName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbUName;
        private System.Windows.Forms.TextBox tbUId;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnDB;
        private System.Windows.Forms.Button btnSetup;
    }
}
