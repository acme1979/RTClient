using System;
using System.Data;
using System.Windows.Forms;
using BaseModel;

namespace WorkStation
{
    public partial class UserLimitConfirmFrm : CForm
    {
        #region Properities && Members
        public UserLimitConfirmFrm()
        {
            InitializeComponent();
            InitCtl();
        }
        #endregion

        #region InitCtl()
        private void InitCtl()
        {
            StyleHelper.Instance.SetStyle(btnLogin);
            StyleHelper.Instance.SetStyle(btnCancel);
            btnLogin.Click += new EventHandler(btnLogin_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            txtUserName.KeyPress += new KeyPressEventHandler(txtUserName_KeyPress);
            txtUserPwd.KeyPress += new KeyPressEventHandler(txtUserPwd_KeyPress);
        }
        #endregion

        #region txtUserPwd_KeyPress
        private void txtUserPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }
        #endregion

        #region txtUserName_KeyPress
        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtUserPwd.Text = "";
                txtUserPwd.Focus();
            }
        }
        #endregion

        #region btnLogin_Click
        private void btnLogin_Click(object sender, EventArgs e)
        {
          
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        #endregion
    }
}
