namespace BaseModel
{
    partial class ErrorInfoFrm
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
            this.dgvErrorInfo = new System.Windows.Forms.DataGridView();
            this.bsErrorInfo = new System.Windows.Forms.BindingSource();
            this.dsErrorInfo1 = new BaseModel.DsErrorInfo();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrorInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsErrorInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsErrorInfo1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvErrorInfo
            // 
            this.dgvErrorInfo.AllowUserToAddRows = false;
            this.dgvErrorInfo.AllowUserToDeleteRows = false;
            this.dgvErrorInfo.AutoGenerateColumns = false;
            this.dgvErrorInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrorInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dgvErrorInfo.DataSource = this.bsErrorInfo;
            this.dgvErrorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvErrorInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvErrorInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dgvErrorInfo.Name = "dgvErrorInfo";
            this.dgvErrorInfo.RowTemplate.Height = 23;
            this.dgvErrorInfo.Size = new System.Drawing.Size(1112, 431);
            this.dgvErrorInfo.TabIndex = 0;
            // 
            // bsErrorInfo
            // 
            this.bsErrorInfo.DataMember = "ErrorInfo";
            this.bsErrorInfo.DataSource = this.dsErrorInfo1;
            // 
            // dsErrorInfo1
            // 
            this.dsErrorInfo1.DataSetName = "DsErrorInfo";
            this.dsErrorInfo1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ErrorCode";
            this.dataGridViewTextBoxColumn4.HeaderText = "ErrorCode";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ErrorMessage";
            this.dataGridViewTextBoxColumn5.HeaderText = "ErrorMessage";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 300;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Other";
            this.dataGridViewTextBoxColumn6.HeaderText = "Other";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 300;
            // 
            // ErrorInfoFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1112, 431);
            this.Controls.Add(this.dgvErrorInfo);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ErrorInfoFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "错误信息显示";
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrorInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsErrorInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsErrorInfo1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvErrorInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorMessageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn otherDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource bsErrorInfo;
        private DsErrorInfo dsErrorInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DsErrorInfo dsErrorInfo1;
    }
}