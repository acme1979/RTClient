using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using BaseModel;
using ConfigHelper;
using System.IO;

namespace RTClient
{
    public partial class DBSetupFrm : Form
    {
        #region 成员变量
        private ConnectionStringSettingsCollection connectionStringList = ConfigurationManager.ConnectionStrings;
        Hashtable htConnectionString = new Hashtable();
        #endregion

        #region 构造函数
        public DBSetupFrm()
        {
            InitializeComponent();
            InitCtl();
        }
        #endregion

        #region InitCtl();
        private void InitCtl()
        {
            StyleHelper.Instance.SetStyle(btnSave);
            StyleHelper.Instance.SetStyle(btnTest);
            this.Load += new EventHandler(DBSettingFrm_Load);
            this.cbxDBName.SelectedIndexChanged += new EventHandler(cbxDBName_SelectedIndexChanged);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            btnTest.Click += new EventHandler(btnTest_Click);
            btnSelect.Click += new EventHandler(btnSelect_Click);
        }
        #endregion

        #region btnSelect_Click
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            if (dlgOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = dlgOpen.FileName;
            }
        }
        #endregion

        #region btnTest_Click
        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                EDBType dbTypeCode = (EDBType)Enum.Parse(typeof(EDBType), cbxDBType.Text);

                IDBHelper helper = DBHelper.Instance.TestConnection(dbTypeCode, tbServerName.Text.Trim(), tbDBName.Text.Trim(), tbUserName.Text.Trim(), 
                    tbPassword.Text.Trim(),tbPort.Text.Trim(), tbPath.Text.Trim());
                if (helper != null)
                {
                    MessageBox.Show("测试成功!");
                }
                else
                {
                    MessageBox.Show("数据库连接失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region DBSettingFrm_Load
        private void DBSettingFrm_Load(object sender, EventArgs e)
        {
            cbxDBType.Items.Clear();
            foreach (string typeName in Enum.GetNames(typeof(EDBType)))
            {
                cbxDBType.Items.Add(typeName);
            }

            cbxDBName.Items.Clear();
            List<string> sections = IniSetupFileHelper.Instance.ReadSections();
            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i].StartsWith("DB-"))
                {
                    cbxDBName.Items.Add(sections[i]);
                }
            }
            if (cbxDBName.Items.Count > 0)
            {
                cbxDBName.SelectedIndex = 0;
            }
        }
        #endregion

        #region ChangShowValue
        private void ChangeShowValue(string dbname)
        {
            Hashtable ht = htConnectionString[dbname] as Hashtable;
            tbServerName.Text = ht["Data Source"].ToString();
            tbDBName.Text = ht["Initial Catalog"].ToString();
            tbUserName.Text = ht["User ID"].ToString();
            tbPassword.Text = ht["Password"].ToString();
        }
        #endregion

        #region cbxDBName_SelectedIndexChanged
        private void cbxDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxDBType.SelectedIndex = cbxDBType.FindString(DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(cbxDBName.Text, "DBTYPE")));
            tbServerName.Text = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(cbxDBName.Text, "SERVER"));
            tbDBName.Text = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(cbxDBName.Text, "DBNAME"));
            tbUserName.Text = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(cbxDBName.Text, "UserID"));
            tbPassword.Text = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(cbxDBName.Text, "Password"));
            tbPort.Text = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(cbxDBName.Text, "PORT"));
            tbPath.Text = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(cbxDBName.Text, "DbFilePath"));
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EDBType dbTypeCode = (EDBType)Enum.Parse(typeof(EDBType), cbxDBType.Text);

                IDBHelper helper = DBHelper.Instance.TestConnection(dbTypeCode, tbServerName.Text.Trim(), tbDBName.Text.Trim(), tbUserName.Text.Trim(), tbPassword.Text.Trim(),
                    tbPort.Text.Trim(), tbPath.Text.Trim());
                if (helper == null)
                {
                    MessageBox.Show("数据库连接失败，请测试通过后保存！");
                    return;
                }
                Save();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Save()
        private void Save()
        {
            List<SetupParamContext> spcList = IniSetupFileHelper.Instance.FindParamList(cbxDBName.Text);
            foreach (SetupParamContext spc in spcList)
            {
                switch (spc.Key)
                {
                    case "DBTYPE": spc.SetValue(DESEncryption.Instance.Encrypt(cbxDBType.Text)); break;
                    case "SERVER": spc.SetValue(DESEncryption.Instance.Encrypt(tbServerName.Text)); break;
                    case "DBNAME": spc.SetValue(DESEncryption.Instance.Encrypt(tbDBName.Text)); break;
                    case "UserID": spc.SetValue(DESEncryption.Instance.Encrypt(tbUserName.Text)); break;
                    case "Password": spc.SetValue(DESEncryption.Instance.Encrypt(tbPassword.Text)); break;
                    case "PORT": spc.SetValue(DESEncryption.Instance.Encrypt(tbPort.Text)); break;
                    case "DbFilePath": spc.SetValue(DESEncryption.Instance.Encrypt(tbPath.Text)); break;
                }
            }
            IniSetupFileHelper.Instance.Save();
            //IniFileHelper.WriteIniData(cbxDBName.Text, "DBTYPE", DESEncryption.Instance.Encrypt(cbxDBType.Text), inifile);
            //IniFileHelper.WriteIniData(cbxDBName.Text, "SERVER", DESEncryption.Instance.Encrypt(tbServerName.Text), inifile);
            //IniFileHelper.WriteIniData(cbxDBName.Text, "DBNAME", DESEncryption.Instance.Encrypt(tbDBName.Text), inifile);
            //IniFileHelper.WriteIniData(cbxDBName.Text, "UserID", DESEncryption.Instance.Encrypt(tbUserName.Text), inifile);
            //IniFileHelper.WriteIniData(cbxDBName.Text, "Password", DESEncryption.Instance.Encrypt(tbPassword.Text), inifile);
            //IniFileHelper.WriteIniData(cbxDBName.Text, "PORT", DESEncryption.Instance.Encrypt(tbPort.Text), inifile);
            //string dbname = cbxDBName.Text.Trim();
            //Hashtable ht = htConnectionString[dbname] as Hashtable;
            //ht["Data Source"] = tbServerName.Text.Trim();
            //ht["Initial Catalog"] = tbDBName.Text.Trim();
            //ht["User ID"] = tbUserName.Text.Trim();
            //ht["Password"] = tbPassword.Text.Trim();
            //htConnectionString[dbname] = ht;
            //string connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}"
            //    , ht["Data Source"].ToString(), ht["Initial Catalog"].ToString(), ht["User ID"].ToString(),
            //    ht["Password"].ToString());
            //UpdateConnectionStringsConfig(dbname, connectionString);
        }
        #endregion

        #region UpdateConnectionStringsConfig
        /// <summary>
        /// 更新字符串连接
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="ConString"></param>
        //private static void UpdateConnectionStringsConfig(string dbName,string ConString)
        //{
        //    bool isModified = false;
        //    if (ConfigurationManager.ConnectionStrings[dbName] != null)
        //    {
        //        isModified = true;
        //    }

        //    ConnectionStringSettings mySettings =
        //        new ConnectionStringSettings(dbName, ConString, "System.Data.SqlClient");
        //    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    if (isModified)
        //    {
        //        config.ConnectionStrings.ConnectionStrings.Remove(dbName);
        //    }
        //    config.ConnectionStrings.ConnectionStrings.Add(mySettings);
        //    config.Save(ConfigurationSaveMode.Modified);
        //    ConfigurationManager.RefreshSection("connectionStrings");

        //}
        #endregion
    }
}
