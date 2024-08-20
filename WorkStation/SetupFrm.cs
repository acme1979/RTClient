using System;
using System.Data;
using System.Windows.Forms;
using BaseModel;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;


namespace WorkStation
{
    public partial class SetupFrm : Form
    {
        private CParamSetting m_ParamSetting;
        public CParamSetting ParamSetting
        {
            get
            {
                if (m_ParamSetting == null)
                {
                    m_ParamSetting = new CParamSetting();
                }
                return m_ParamSetting;
            }
            set
            {
                m_ParamSetting = value;
            }
        }
        IDBHelper dbHelper = null;

        public SetupFrm()
        {
            InitializeComponent();
            this.Load += new EventHandler(SetupFrm_Load);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            txtName.KeyDown += new KeyEventHandler(txtName_KeyDown);
            cmbDATA_AUTH.SelectedIndexChanged += new EventHandler(cmbDATA_AUTH_SelectedIndexChanged);
            cmbWorkStation.SelectedIndexChanged += new EventHandler(cmbWorkStation_SelectedIndexChanged);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtName.Text.Trim() != "")
                {
                    btnSave_Click(null,null);
                }
            }
        }



        #region 员工校验
        private bool checkName()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("请输入工号！");
                return false;
            }
            List<DBParameters> ListDBParameters = new List<DBParameters>();
            dbHelper.AddDBParameters(ListDBParameters, "DATA_AUTH", cmbDATA_AUTH.SelectedValue.ToString(), ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_EMP", txtName.Text.Trim(), ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "RES", null, ParameterDirection.Output, valueTypes.STRING);

            Hashtable htUser = dbHelper.htExecuteNonQuery(ListDBParameters, "P_C_CHECK_EMP");//存储中增加名称参数
            if (!htUser["RES"].ToString().StartsWith("OK"))
            {
                MessageBox.Show(htUser["RES"].ToString());
                return false;
            }
            string sql = "SELECT T.NAME FROM SY_USER T LEFT JOIN SY_DATA_AUTH A  ON T.ID = A.USER_ID " +
             "WHERE T.LOGIN_NAME = '" + txtName.Text.Trim() + "' AND A.DEPT_ID = '" + cmbDATA_AUTH.SelectedValue.ToString() + "' ";

            if ("DB-PGSQL".Equals(m_ParamSetting.DBCode))
                sql += " LIMIT 1 ";
            else
                sql += " AND ROWNUM = 1 ";
            DataTable empDT = dbHelper.GetDataTable(sql, "SY_USER");
            txtName.Tag = empDT.Rows[0]["NAME"].ToString();
            return true;
        }
        #endregion

        #region SetupFrm_Load
        private void SetupFrm_Load(object sender, EventArgs e)
        {
            try
            {
                dbHelper = DBHelper.Instance.GetDBInstace(m_ParamSetting.DBCode);
                ComboDataAuth();
            }
            catch (Exception ex)
            {
                MessageBox.Show("信息加载错误：" + ex.Message);
            }
        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (ParamSetting == null || ParamSetting.Process == null)
            {
                if (MessageBox.Show("参数尚未设置，取消后系统将退出，确认取消吗？","提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                Application.Exit();
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbWorkStation.ToString() == "")
                {
                    MessageBox.Show("请选择工作中心！");
                    return;
                }
                if (!checkName())
                {
                    return;
                }

                ParamSetting.DATA_AUTH = cmbDATA_AUTH.SelectedValue.ToString();
                ParamSetting.DATA_AUTHNAME = cmbDATA_AUTH.Text;
                ParamSetting.WorkStation = cmbWorkStation.SelectedValue.ToString();
                ParamSetting.WorkStationName = cmbWorkStation.Text;
                ParamSetting.ProductLine = cmbLine.SelectedValue.ToString();
                ParamSetting.ProductLineName = cmbLine.Text;

                ParamSetting.Process = cmdGroupCode.SelectedValue.ToString();
                ParamSetting.ProcessName = cmdGroupCode.Text;
                ParamSetting.EmpCode = txtName.Text.Trim();
                ParamSetting.EmpName = txtName.Tag.ToString();

                ParamSetting.Serializer(ParamSetting);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置异常：" + ex.Message);
            }
        }
        #endregion

        #region lueProcess_TextChanged
        private void lueProcess_TextChanged(object sender, EventArgs e)
        {
            //if (lueWorkStation.EditValue != null)
            //{
            //    string sql = "SELECT AA01,AA02 FROM BASAA WHERE ISNULL(AA25,0) = 1 AND AA06 = '" + lueProcess.EditValue.ToString() + "'";
            //    DataSet ds = new DataSet ();
            //    lueWorkStation.EditValue = "AA01";
            //    lueWorkStation.Properties.ValueMember = "AA01";
            //    lueWorkStation.Properties.DisplayMember = "AA02";
            //    lueWorkStation.Properties.DataSource = ds.Tables["BASAA"];
            //    //自适应宽度
            //    lueWorkStation.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //    //填充列
            //    lueWorkStation.Properties.PopulateColumns();
            //    //设置列属性
            //    lueWorkStation.Properties.Columns[0].Caption = "工站代码";
            //    lueWorkStation.Properties.Columns[1].Caption = "工站名称";
            //}
        }
        #endregion

        #region btnChangeToManage_Click
        private void btnChangeToManage_Click(object sender, EventArgs e)
        {
            //AppConfigHelper appset = new AppConfigHelper();
            //appset.SetAppValue("Client", "false");
            //appset.SetAppValue("ClientType", "PRODUCT");
            //appset.SetAppValue("ShowParam", "PRODUCT");

            this.Dispose();
            GC.Collect();
            Application.Exit();

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "RTCLient.exe";
            System.Diagnostics.Process.Start(startInfo);
        }
        #endregion

        #region btnChangeToRepair_Click
        private void btnChangeToRepair_Click(object sender, EventArgs e)
        {
            //AppConfigHelper appset = new AppConfigHelper();
            //appset.SetAppValue("Client", "true");
            //appset.SetAppValue("ClientType", "REPAIR");
            //appset.SetAppValue("ShowParam", "REPAIR");

            this.Dispose();
            GC.Collect();
            Application.Exit();

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "RTCLient.exe";
            System.Diagnostics.Process.Start(startInfo);
        }
        #endregion

        #region 基础数据加载

        #region 组织机构填充
        /// <summary>
        /// 组织机构填充
        /// </summary>
        private void ComboDataAuth()
        {
            try
            {
                string sql = "SELECT NAME,ID FROM SY_DEPT WHERE IS_AUTH = '1' ";//格兰博：DATA_AUTH 
                if (ParamSetting.DATA_AUTH != "")
                    sql += " AND ID = '" + ParamSetting.DATA_AUTH + "' ";
                DataTable dataAuth = dbHelper.GetDataTable(sql, "SY_DEPT");
                this.cmbDATA_AUTH.DataSource = dataAuth;
                cmbDATA_AUTH.DisplayMember = "NAME";
                cmbDATA_AUTH.ValueMember = "ID";//DATA_AUTH
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常:" + ex.Message);
            }
            cmbDATA_AUTH_SelectedIndexChanged(null, null);
        }
        #endregion

        #region 组织机构改变,重新加载工作中心
        private void cmbDATA_AUTH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDATA_AUTH.SelectedIndex == -1)
            {
                return;
            }
            if (cmbDATA_AUTH.SelectedValue.ToString() == "System.Data.DataRowView")
            {
                return;
            }
            //加载工作中心
            DataTable dtArea = GetArea(cmbDATA_AUTH.SelectedValue.ToString());
            if (dtArea.Rows.Count == 0)
                return;
            this.cmbWorkStation.DataSource = dtArea;
            cmbWorkStation.DisplayMember = "CA_NAME";
            cmbWorkStation.ValueMember = "CA_ID"; 
            cmbWorkStation.SelectedIndex = -1;
        }
        #endregion

        #region 通过组织机构获取工作中心
        /// <summary>
        ///  通过组织机构获取工作中心
        /// </summary>
        /// <param name="cmbLine"></param>
        /// <param name="cmbDATA_AUTH"></param>
        /// <returns></returns>
        private DataTable GetArea(string cmbDATA_AUTH)
        {
            //string sql = "SELECT A.CA_ID,A.CA_NAME FROM T_CO_AREA A " +
            //             "WHERE A.CA_TYPE='2' AND A.CA_STATUS = 'Y' AND A.DATA_AUTH = '" + cmbDATA_AUTH + "' ";
            string sql = "SELECT A.CA_ID,A.CA_NAME FROM T_CO_AREA A LEFT JOIN T_CO_GROUP B ON A.CA_GROUP = B.GROUP_CODE AND A.DATA_AUTH = B.DATA_AUTH " +
                         "WHERE A.CA_TYPE='2' AND A.CA_STATUS = 'Y'  AND A.DATA_AUTH = '" + cmbDATA_AUTH + "' AND B.GROUP_WORKSTATION IS NOT NULL OR B.GROUP_WORKSTATION <> '' ORDER BY CA_NAME";
            DataTable dtArea = dbHelper.GetDataTable(sql, "T_CO_AREA");
            return dtArea;
        }
        #endregion

        #region 工作中心改变，获取线体
        private void cmbWorkStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWorkStation.SelectedIndex == -1)
            {
                return;
            }
            if (cmbWorkStation.SelectedValue.ToString() == "System.Data.DataRowView")
            {
                return;
            }
            string sql = "SELECT A.CA_PARENTAREAID ,C.CA_NAME ,A.CA_GROUP,B.GROUP_NAME FROM T_CO_AREA A  " +
                         "  LEFT JOIN T_CO_GROUP B ON A.CA_GROUP = B.GROUP_CODE AND A.DATA_AUTH = B.DATA_AUTH " +
                         "  LEFT JOIN T_CO_AREA C ON A.CA_PARENTAREAID = C.CA_ID AND A.DATA_AUTH = C.DATA_AUTH " + 
                         "WHERE A.CA_TYPE='2' AND A.CA_ID='" + cmbWorkStation.SelectedValue.ToString() +
                         "' AND A.DATA_AUTH = '" + cmbDATA_AUTH.SelectedValue.ToString() + "' ";
            DataTable dtLine = dbHelper.GetDataTable(sql, "T_CO_AREA");
            if (dtLine.Rows.Count == 1)
            {
                this.cmbLine.DataSource = dtLine;
                cmbLine.DisplayMember = "CA_NAME";
                cmbLine.ValueMember = "CA_PARENTAREAID";

                this.cmdGroupCode.DataSource = dtLine;
                cmdGroupCode.DisplayMember = "GROUP_NAME";
                cmdGroupCode.ValueMember = "CA_GROUP";
            }
        }
        #endregion

        #endregion
    }
}
