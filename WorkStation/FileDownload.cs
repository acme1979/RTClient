using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseModel;
using System.Collections;
using WorkStation.FunClass;
using System.IO;

namespace WorkStation
{
    public partial class FileDownload : CUserControl,ICommonWorkStationCtl
    {
        #region 构造函数与成员  get、set
        private string m_PO;
        /// <summary>
        /// 当前生产单据号
        /// </summary>
        public string PO
        {
            get { return m_PO; }
            set { m_PO = value; }
        }

        private string m_Process;
        /// <summary>
        /// 当前生产工序代码
        /// </summary>
        public string Process
        {
            get { return m_Process; }
            set { m_Process = value; }
        }

        private string m_ProcessName;
        /// <summary>
        /// 当前生产工序代码
        /// </summary>
        public string ProcessName
        {
            get { return m_ProcessName; }
            set { m_ProcessName = value; }
        }

        private string m_ProductLine;
        /// <summary>
        /// 线别
        /// </summary>
        public string ProductLine
        {
            get { return m_ProductLine; }
            set { m_ProductLine = value; }
        }

        private string m_ProductLineName;
        /// <summary>
        /// 线别
        /// </summary>
        public string ProductLineName
        {
            get { return m_ProductLineName; }
            set { m_ProductLineName = value; }
        }
        private string m_WorkStation;
        /// <summary>
        /// 当前生产工站代码
        /// </summary>
        public string WorkStation
        {
            get { return m_WorkStation; }
            set { m_WorkStation = value; }
        }

        private string m_WorkStationName;
        /// <summary>
        /// 当前生产工站名称
        /// </summary>
        public string WorkStationName
        {
            get { return m_WorkStationName; }
            set { m_WorkStationName = value; }
        }

        private string m_UsrCode;
        /// <summary>
        /// 当前用户工号
        /// </summary>
        public string UsrCode
        {
            get { return m_UsrCode; }
            set { m_UsrCode = value; }
        }

        private string m_UsrName;
        /// <summary>
        /// 当前用户姓名
        /// </summary>
        public string UsrName
        {
            get { return m_UsrName; }
            set { m_UsrName = value; }
        }

        /// <summary>
        /// 界面显示模块列表
        /// </summary>
        List<string> m_GroupBoxShowList = new List<string>();
        public List<string> GroupBoxShowList
        {
            get { return m_GroupBoxShowList; }
            set { }
        }
        /// <summary>
        /// 系统控件对接的通用参数集合
        /// </summary>
        private Hashtable htCommonParam = null;
        public Hashtable HtCommonParam
        {
            get
            {
                if (htCommonParam == null)
                { htCommonParam = new Hashtable(); }
                return htCommonParam;
            }
            set { htCommonParam = value; }
        }

        private string dbCode;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBCode
        {
            get { return dbCode; }
            set { dbCode = value; }
        }

        private IDBHelper dbHelper;
        /// <summary>
        /// 数据库链接
        /// </summary>
        public IDBHelper iDBHelper
        {
            get { return dbHelper; }
            set { dbHelper = value; }
        }

        private string data_auth;
        /// <summary>
        /// 组织机构
        /// </summary>
        public string DATA_AUTH
        {
            get { return data_auth; }
            set { data_auth = value; }
        }

        #endregion

        /// <summary>
        /// WorkStation.ini 配置文件路径
        /// </summary>
        string wsIniPath = Application.StartupPath + @"\Config\WorkStation.ini";
        /// <summary>
        /// 当前IP
        /// </summary>
        string ip = "";
        /// <summary>
        ///声音控制
        /// </summary>
        SoundPlayerHelp sound = new SoundPlayerHelp();
        /// <summary>
        ///PLM数据库连接
        /// </summary>
        DBPGSQLHelper PgSQLHelper = null;

        FileTransferService filedownload = new FileTransferService();

        string localPath = "";
        string itemCode = "";
        string softCode = "";

        /// <summary>
        /// 根据路径删除文件
        /// </summary>
        /// <param name="path"></param>
        public void DeleteFile(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            else
            {
                File.Delete(path);
            }
        }

        public FileDownload()
        {
            InitializeComponent();
            this.Load += new EventHandler(FileDownload_Load);
            this.timerMusic.Tick += new EventHandler(timerMusic_Tick);
            tbLocalPath.Click += new EventHandler(tbLocalPath_Click);
            tbItemCode.KeyDown += new KeyEventHandler(tbItemCode_KeyDown);
            btnDownload.Click += new EventHandler(btnDownload_Click);
        }

        #region InitData
        public void InitData()
        {
            tbLocalPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath", "", wsIniPath);
            ip = Computer.Instance().IpAddress;
        }
        #endregion

        #region frmLOAD
        private void FileDownload_Load(object sender, EventArgs e)
        {
            lblItemName.Text = "";
            lblItemSpec.Text = "";
            try
            {
                PgSQLHelper = new DBPGSQLHelper("192.168.101.131", "LDPLM", "readonly", "ropass");
                //PgSQLHelper = new DBPGSQLHelper("192.168.101.130", "PLM0616", "mestest2", "MESTEST2");
                lblMsg("OK", "OK：PLM服务器连接成功！");
                LogRefresh();
            }
            catch(Exception)
            {
                lblMsg("NG", "NG：PLM服务器连接失败！");
                return;
            }
        }
        #endregion

        private void LogRefresh()
        {
            DataTable dt01 = LogsSelectReturn(data_auth, WorkStation);
            dataGridView1.DataSource = dt01;
        }

        #region ExecScanBarOperate
        public CScanResult ExecScanBarOperate(string BarString)
        {
            CScanResult csr = new CScanResult();
            csr.BarString = BarString;
            csr.Result = "NG";
            csr.Remark = "NG：不符合流程";
            return csr;
        }
        #endregion

        #region 本地路径选取
        private void tbLocalPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个文件夹";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbLocalPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ReadPath", this.tbLocalPath.Text, wsIniPath);
            }
        }
        #endregion

        #region 下载按钮点击
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if ("".Equals(tbLocalPath.Text))
            {
                lblMsg("NG", "NG：本地路径为空，请选择！");
                return;
            }
            if ("".Equals(tbItemCode.Text))
            {
                lblMsg("NG", "NG：料号为空，请输入！");
                return;
            }
            if ("".Equals(cbbSoftItemCode.Text))
            {
                lblMsg("NG", "NG：软件料号为空，请重新选择！");
                return;
            }
            softCode = cbbSoftItemCode.Text;
            localPath = tbLocalPath.Text;
            DeleteFile(localPath);
            DataTable dt01 = PLMFilePathReturn(softCode);
            if (dt01.Rows.Count < 1)
            {
                lblMsg("NG", "NG：PLM中无" + softCode + "资料！");
                return;
            }
            dataGridView1.DataSource = dt01;
            string plmpath = @"\\192.168.101.131\SIPM21\";
            string filepath = @"\\192.168.101.131" + dt01.Rows[0]["location"].ToString();
            string localpath = tbLocalPath.Text + @"\";
            OperateResult or = filedownload.TransferFileFromServer(filepath, localpath, plmpath, @"mes", @"plm@2023");
            string logmsg = @"WORK_STATION=" + WorkStation + @"," + @"MODEL_NAME=" + tbItemCode.Text + @"," + @"FILE_MODE=" + cbbSoftItemCode.Text + @"," + @"SOURCE_PATH=" + filepath;
            if (or.IsSuccess)
            {
                lblMsg("OK", or.Message);
                SaveLogs(UsrCode, data_auth, or.Message, logmsg, "文件下载", "true");
            }
            else
            {
                lblMsg("NG", or.Message);
                SaveLogs(UsrCode, data_auth, "失败", or.Message, "文件下载", "true");
            }
            LogRefresh();
        }
        #endregion

        #region 物料号回车
        private void tbItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbItemCode.Text.Trim() != "")
                {
                    DataTable dt01 = ItemSelectedReturn(tbItemCode.Text);
                    if (dt01.Rows.Count < 1)
                    {
                        lblMsg("NG", "NG：该物料号不存在，请重新输入！");
                        lblItemName.Text = "";
                        lblItemSpec.Text = "";
                        cbbSoftItemCode.DataSource = null;
                        tbItemCode.Focus();
                        tbItemCode.SelectAll();
                        return;
                    }
                    else
                    {
                        lblItemName.Text = dt01.Rows[0]["CI_ITEM_NAME"].ToString();
                        lblItemSpec.Text = dt01.Rows[0]["CI_ITEM_SPEC"].ToString();
                        DataTable dt02 = CoTechItemSelectReturn(tbItemCode.Text);
                        if (dt02.Rows.Count < 1)
                        {
                            lblMsg("NG", "NG：该产品未维护管控软件料号！");
                            tbItemCode.Focus();
                            tbItemCode.SelectAll();
                            cbbSoftItemCode.DataSource = null;
                            return;
                        }
                        cbbSoftItemCode.DataSource = dt02;
                        cbbSoftItemCode.DisplayMember = "CTI_ITEM_CODE";
                        cbbSoftItemCode.SelectedIndex = 0;
                        itemCode = tbItemCode.Text;
                        lblMsg("OK", "OK：请选择软件料号！");
                    }
                }
            }
        }
        #endregion

        #region lblMsg
        private void lblMsg(string status, string msg)
        {
            lblResState.Text = msg;
            if ("OK".Equals(status))
            {
                lblResState.BackColor = Color.Green;
                sound.passPlayer.PlayLooping();//播放通过提示音
            }
            else
            {
                lblResState.BackColor = Color.Red;
                sound.sndplayer.PlayLooping();//播放通过提示音
            }
            timerMusic.Interval = 2000;
            timerMusic.Enabled = true;
        }
        #endregion

        #region timerMusic_Tick
        private void timerMusic_Tick(object sender, EventArgs e)
        {
            sound.passPlayer.Stop();
            sound.sndplayer.Stop();
            timerMusic.Enabled = false;
        }
        #endregion

        #region SQL
        private DataTable ItemSelectedReturn(string itemcode)
        {
            string sql = "SELECT CI_ITEM_CODE , CI_ITEM_NAME , CI_ITEM_SPEC FROM T_CO_ITEM WHERE CI_ITEM_CODE = '" + itemcode + "' AND DATA_AUTH = '" + data_auth + "'";
            DataTable dt = dbHelper.GetDataTable(sql, "T_CO_ITEM");
            return dt;
        }

        private DataTable CoTechItemSelectReturn(string modelcode)
        {
            string sql = "SELECT CTI_MODEL_CODE , CTI_ITEM_CODE , CTI_GROUP FROM T_CO_TECH_ITEM WHERE CTI_ITEM_TYPE = '3' AND CTI_MODEL_CODE = '" + modelcode + "' AND DATA_AUTH = '" + data_auth + "'";
            DataTable dt = dbHelper.GetDataTable(sql, "T_CO_TECH_ITEM");
            return dt;
        }

        private DataTable PLMFilePathReturn(string no)
        {
            string sql = "SELECT a.no , c.fname , c.location  FROM mpart a , sipm21_objof b , sipm21 c " +
                "WHERE a.id = b.itemid1 AND b.itemid2 = c.id " +
                "and a.del = 0 and a.wkaid <> '3' " +
                "and b.del = 0 and b.wkaid <> '3' " +
                "and c.del = 0 and c.wkaid <> '3' " +
                "and a.no = '" + no + "'";
            DataTable dt = PgSQLHelper.GetDataTable(sql, "mpart");
            return dt;
        }

        private DataTable LogsSelectReturn(string dataauth, string workstation)
        {
            string sql = string.Format(@"SELECT T.CREATE_TIME 执行时间 , T.ERRORMESSAGE 反馈信息 , T.LOG_DATA_INFO 日志信息 FROM T_XC_SYNC_LOG T
                                         WHERE DATA_AUTH = '{0}' AND LOG_FUNCTION_NAME = '文件下载' AND LOG_DATA_INFO LIKE '%WORK_STATION={1}%' AND T.CREATE_TIME >= TRUNC(SYSDATE - 3)
                                         ORDER BY CREATE_TIME DESC", dataauth, workstation);
            DataTable dt = dbHelper.GetDataTable(sql, "T_CO_TECH_ITEM");
            return dt;
        }

        private void SaveLogs(string usrcode , string dataauth , string message , string datainfo , string funname , string resflag)
        {
            string sql = string.Format(@"INSERT INTO T_XC_SYNC_LOG 
                                        (ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ERRORDATE, ERRORMESSAGE, LOG_DATA_INFO, LOG_FUNCTION_NAME, RESULT_FLAG)
                                        VALUES(F_C_GETNEWID , '{0}' , SYSDATE , '{1}' , SYSDATE , '{2}' , '{3}' , '{4}' , '{5}')"
                                        , usrcode , dataauth , message , datainfo , funname , resflag);
            dbHelper.ExecuteSql(sql);
        }
        #endregion

        #region 共享文件夹
        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.StandardInput.WriteLine("net use * /del /y");
                string dosLine = "net use " + path + " " + passWord + " /user:" + userName;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    MessageBox.Show(errormsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
        #endregion
    }
}
