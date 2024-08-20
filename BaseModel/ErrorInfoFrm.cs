using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaseModel
{
    public partial class ErrorInfoFrm : Form
    {
        #region 成员变量
        private DataSet m_DsErrorInfo;
        public DataSet M_DsErrorInfo
        {
            get
            {
                if (m_DsErrorInfo == null)
                {
                    m_DsErrorInfo = this.dsErrorInfo;
                }
                return this.m_DsErrorInfo;
            }
            set
            {
                if (value != null)
                {
                    //this.dsErrorInfo.Clear();
                    //this.dsErrorInfo.AcceptChanges();
                    m_DsErrorInfo.Merge(value);
                    this.dsErrorInfo.Merge(m_DsErrorInfo);
                    this.dsErrorInfo.AcceptChanges();
                }
            }
        }
        #endregion
        public ErrorInfoFrm()
        {
            InitializeComponent();
        }

        #region InitCtl()
        private void InitCtl()
        {
            StyleHelper.Instance.SetStyle(dgvErrorInfo);
        }
        #endregion
    }
}
