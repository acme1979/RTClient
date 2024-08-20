namespace WorkStation
{
    partial class EcuLabel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlScan = new System.Windows.Forms.Panel();
            this.tbScanData = new System.Windows.Forms.TextBox();
            this.lblScanData = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSnLen = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnXG = new System.Windows.Forms.Button();
            this.lblTS = new System.Windows.Forms.Label();
            this.txtOldSn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCustSnLen = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlRltState = new System.Windows.Forms.Panel();
            this.lblRltState = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MES_SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CUST_SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerMusic = new System.Windows.Forms.Timer(this.components);
            this.pnlScan.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlRltState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlScan
            // 
            this.pnlScan.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlScan.Controls.Add(this.tbScanData);
            this.pnlScan.Controls.Add(this.lblScanData);
            this.pnlScan.Controls.Add(this.label2);
            this.pnlScan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlScan.Location = new System.Drawing.Point(0, 0);
            this.pnlScan.Name = "pnlScan";
            this.pnlScan.Size = new System.Drawing.Size(1028, 49);
            this.pnlScan.TabIndex = 3;
            // 
            // tbScanData
            // 
            this.tbScanData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbScanData.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbScanData.Location = new System.Drawing.Point(123, 0);
            this.tbScanData.Margin = new System.Windows.Forms.Padding(2);
            this.tbScanData.Name = "tbScanData";
            this.tbScanData.Size = new System.Drawing.Size(901, 46);
            this.tbScanData.TabIndex = 3;
            // 
            // lblScanData
            // 
            this.lblScanData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblScanData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScanData.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScanData.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblScanData.Location = new System.Drawing.Point(123, 0);
            this.lblScanData.Name = "lblScanData";
            this.lblScanData.Size = new System.Drawing.Size(901, 45);
            this.lblScanData.TabIndex = 2;
            this.lblScanData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblScanData.Visible = false;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 45);
            this.label2.TabIndex = 1;
            this.label2.Text = "刷入条码：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSnLen);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.btnXG);
            this.panel1.Controls.Add(this.lblTS);
            this.panel1.Controls.Add(this.txtOldSn);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtCustSnLen);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 94);
            this.panel1.TabIndex = 4;
            // 
            // txtSnLen
            // 
            this.txtSnLen.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSnLen.Location = new System.Drawing.Point(895, 58);
            this.txtSnLen.Margin = new System.Windows.Forms.Padding(2);
            this.txtSnLen.Name = "txtSnLen";
            this.txtSnLen.Size = new System.Drawing.Size(41, 26);
            this.txtSnLen.TabIndex = 12;
            this.txtSnLen.Text = "18";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(762, 61);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 19);
            this.label4.TabIndex = 11;
            this.label4.Text = "产品条码长度";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(555, 18);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(161, 23);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "不校验产品流程";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnXG
            // 
            this.btnXG.Location = new System.Drawing.Point(953, 13);
            this.btnXG.Margin = new System.Windows.Forms.Padding(2);
            this.btnXG.Name = "btnXG";
            this.btnXG.Size = new System.Drawing.Size(59, 31);
            this.btnXG.TabIndex = 9;
            this.btnXG.Text = "修改";
            this.btnXG.UseVisualStyleBackColor = true;
            // 
            // lblTS
            // 
            this.lblTS.AutoSize = true;
            this.lblTS.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTS.Location = new System.Drawing.Point(74, 22);
            this.lblTS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTS.Name = "lblTS";
            this.lblTS.Size = new System.Drawing.Size(219, 22);
            this.lblTS.TabIndex = 8;
            this.lblTS.Text = "提示：请扫描MES条码";
            // 
            // txtOldSn
            // 
            this.txtOldSn.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOldSn.Location = new System.Drawing.Point(135, 58);
            this.txtOldSn.Margin = new System.Windows.Forms.Padding(2);
            this.txtOldSn.Name = "txtOldSn";
            this.txtOldSn.ReadOnly = true;
            this.txtOldSn.Size = new System.Drawing.Size(338, 32);
            this.txtOldSn.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(74, 61);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 22);
            this.label3.TabIndex = 6;
            this.label3.Text = "旧值";
            // 
            // txtCustSnLen
            // 
            this.txtCustSnLen.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCustSnLen.Location = new System.Drawing.Point(895, 18);
            this.txtCustSnLen.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustSnLen.Name = "txtCustSnLen";
            this.txtCustSnLen.Size = new System.Drawing.Size(41, 26);
            this.txtCustSnLen.TabIndex = 5;
            this.txtCustSnLen.Text = "18";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(762, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "客户条码长度";
            // 
            // pnlRltState
            // 
            this.pnlRltState.BackColor = System.Drawing.Color.Green;
            this.pnlRltState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlRltState.Controls.Add(this.lblRltState);
            this.pnlRltState.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRltState.Location = new System.Drawing.Point(0, 384);
            this.pnlRltState.Name = "pnlRltState";
            this.pnlRltState.Size = new System.Drawing.Size(1028, 102);
            this.pnlRltState.TabIndex = 23;
            // 
            // lblRltState
            // 
            this.lblRltState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRltState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRltState.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRltState.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblRltState.Location = new System.Drawing.Point(0, 0);
            this.lblRltState.Name = "lblRltState";
            this.lblRltState.Size = new System.Drawing.Size(1024, 98);
            this.lblRltState.TabIndex = 3;
            this.lblRltState.Text = "请扫描MES条码";
            this.lblRltState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SN,
            this.MES_SN,
            this.CUST_SN});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridView1.Location = new System.Drawing.Point(0, 143);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1028, 241);
            this.dataGridView1.TabIndex = 24;
            // 
            // SN
            // 
            this.SN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SN.DataPropertyName = "SN";
            this.SN.HeaderText = "";
            this.SN.Name = "SN";
            this.SN.ReadOnly = true;
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SN.Width = 50;
            // 
            // MES_SN
            // 
            this.MES_SN.HeaderText = "MES条码";
            this.MES_SN.Name = "MES_SN";
            this.MES_SN.ReadOnly = true;
            // 
            // CUST_SN
            // 
            this.CUST_SN.HeaderText = "客户条码";
            this.CUST_SN.Name = "CUST_SN";
            this.CUST_SN.ReadOnly = true;
            // 
            // timerMusic
            // 
            this.timerMusic.Interval = 400;
            // 
            // EcuLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pnlRltState);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlScan);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EcuLabel";
            this.Size = new System.Drawing.Size(1028, 486);
            this.pnlScan.ResumeLayout(false);
            this.pnlScan.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlRltState.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlScan;
        private System.Windows.Forms.TextBox tbScanData;
        private System.Windows.Forms.Label lblScanData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtOldSn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCustSnLen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlRltState;
        private System.Windows.Forms.Label lblRltState;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn MES_SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn CUST_SN;
        private System.Windows.Forms.Timer timerMusic;
        private System.Windows.Forms.Label lblTS;
        private System.Windows.Forms.Button btnXG;
        private System.Windows.Forms.TextBox txtSnLen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
