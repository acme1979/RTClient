using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BaseModel;
using System.Configuration;
using System.IO;
using BaseModel.Logs;

/**
 *  读取TXT的数据，走生产测试指令
 *  格兰博指令有些特殊，直接执行P_C_CHECK_EC存储即可
 *  如别的项目，需要执行 P_C_CHECK_EC 完成之后，还要执行产品SN校验、保存存储
 *  鑫联信项目(ICT)功能
 * 
 */
namespace WorkStation
{
    public partial class TimerReadWSCtl : CUserControl, ICommonWorkStationCtl
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
        /// 当前生产工序名称
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
            set 
            {
                m_WorkStation = value;
                txtDbPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Path", "", wsIniPath);
                txtBackPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BackPath", "", wsIniPath);
                tbReadRate.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Fre", "", wsIniPath);
                comsblx.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "MacType", "", wsIniPath);
            }
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
        /// 不良现象列表
        /// </summary>
        List<string> m_BadList = new List<string>();
        public List<string> BadList
        {
            get { return m_BadList; }
            set { m_BadList = value; }
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

        private WorkStationCtl m_ParentCtl;
        /// <summary>
        /// 当前生产工序代码
        /// </summary>
        public WorkStationCtl ParentCtl
        {
            get { return m_ParentCtl; }
            set { m_ParentCtl = value; }
        }

        private string dbCOde;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBCode
        {
            get { return dbCOde; }
            set { dbCOde = value; }
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

        private Boolean isHeartBeat;
        /// <summary>
        /// 是否启用心跳
        /// </summary>
        public Boolean IsHeartBeat
        {
            get { return isHeartBeat; }
            set { isHeartBeat = value; }
        }

        private int heartBeatTime;
        /// <summary>
        /// 心跳时间
        /// </summary>
        public int HeartBeatTime
        {
            get { return heartBeatTime; }
            set { heartBeatTime = value; }
        }
        #endregion

        #region InitData
        /// <summary>
        /// 用于执行控件数据初始化；
        /// </summary>
        public void InitData()
        {
            //m_ParentCtl.ScanProductBarcode();
        }
        #endregion

        DBSQLiteHelper dbSqlite = null;
        /// <summary>
        /// WorkStation.ini 配置文件路径
        /// </summary>
        string wsIniPath = Application.StartupPath + @"\Config\WorkStation.ini";

        /// <summary>
        /// 工序解析类型(SPI、AOI)
        /// </summary>
        public string ProcessAnaType = "";

        public TimerReadWSCtl()
        {
            InitializeComponent();
            this.Load += new EventHandler(TimerReadWSCtl_Load);
        }

        #region TimerReadAccessWSCtl_Load
        private void TimerReadWSCtl_Load(object sender, EventArgs e)
        {
            txtDbPath.Click += new EventHandler(txtDbPath_Click);
            txtBackPath.Click += new EventHandler(txtBackPath_Click);
            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);
            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);
            dataGridView1.Focus();
        }

        #endregion

        #region btnStart_Click
        private void btnStart_Click(object sender, EventArgs e)
        {
            //根据工序获取读取的设备类型
            string sql = "SELECT GROUP_READ_TYPE FROM T_CO_GROUP WHERE GROUP_CODE = '" + Process + "' AND DATA_AUTH = '" + data_auth + "' ";
            DataTable lxdt = dbHelper.GetDataTable(sql, "T_CO_GROUP");
            if (lxdt.Rows.Count != 1)
            {
                MessageBox.Show("未获取到绑定的设备类型！");
                return;
            }
            ProcessAnaType = lxdt.Rows[0]["GROUP_READ_TYPE"].ToString();
            #region AOI校验
            if (ProcessAnaType == "AOI")
            {
                if (comsblx.Text != "德律AOI" && comsblx.Text != "劲拓AOI")
                {
                    MessageBox.Show("AOI请选择【德律】或【劲拓】设备类型！");
                    return;
                }
            }
            #endregion

            IniFileHelper.WriteIniData("workStationNum", WorkStation + "MacType", this.comsblx.Text, wsIniPath);
            IniFileHelper.WriteIniData("workStationNum", WorkStation + "Fre", this.tbReadRate.Text, wsIniPath);
            int readpl = 5;
            int.TryParse(tbReadRate.Text, out readpl);
            if (readpl == 0)
                readpl = 5;
            timer1.Enabled = true;
            timer2.Interval = readpl * 1000;
            timer2.Enabled = true;
            SetEnable();
        }
        #endregion

        #region btnEnd_Click
        private void btnEnd_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            lblState.Text = "系统监听已停止...";
            lblStateTitle.ForeColor = Color.Red;
            lblState.ForeColor = Color.Red;
            SetEnable();
        }
        #endregion

        #region SetEnable()
        private void SetEnable()
        {
            btnEnd.Enabled = !btnEnd.Enabled;
            btnStart.Enabled = !btnStart.Enabled;
            tbReadRate.ReadOnly = !tbReadRate.ReadOnly;
       }
        #endregion

        #region 选择读取路径
        private void txtDbPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个文件";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.txtDbPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "Path", this.txtDbPath.Text, wsIniPath);
            }
        }

        private void txtBackPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个备份文件夹";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.txtBackPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "BackPath", this.txtBackPath.Text, wsIniPath);
            }
        }
        #endregion

        #region timer1_Tick
        int iTimerFlag = 0;
        string iTimerTitle = "系统正在监听,数据获取中...";
        private void timer1_Tick(object sender, EventArgs e)
        {
            iTimerFlag++;
            if (iTimerFlag > 15)
            {
                iTimerFlag = 1;
            }
            lblState.Text = iTimerTitle.Substring(0, iTimerFlag);
        }
        #endregion

        #region timer2_Tick
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            timer2.Enabled = MachineDataRead();
        }
        #endregion

        #region MachineDataRead
        private bool MachineDataRead()
        {
            bool isTrue = false;
            switch (ProcessAnaType)
            {
                case "SPI":
                    isTrue = readSpiData();
                    break;
                case "AOI":
                    if (comsblx.Text == "劲拓AOI")
                        isTrue = readAoiJtData();
                    else if (comsblx.Text == "德律AOI")
                        isTrue = readAoiDlData();
                    break;
                default:
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    lblState.Text = "没有符合数据解析的工序类型...";
                    lblStateTitle.ForeColor = Color.Red;
                    lblState.ForeColor = Color.Red;
                    SetEnable();
                    isTrue = false;
                    break;
            }
            return isTrue;
        }
        #endregion

        #region 存储
        /// <summary>
        /// 生产测试指令-Oracle与pg的存储有点区别
        /// </summary>
        /// <param name="ListDBParameters"></param>
        /// <param name="SN">条码SN</param>
        /// <param name="ecCode">不良代码</param>
        /// <param name="psCode">不良点数</param>
        /// <param name="ht"></param>
        /// <returns></returns>
        private Hashtable run(List<DBParameters> ListDBParameters, string SN, string ecCode,string psCode)
        {
            dbHelper.AddDBParameters(ListDBParameters, "M_DATA_AUTH", data_auth, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_SN", SN, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_WORK_STATIONID", WorkStation, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_EMP", m_UsrCode, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_FLAG", null, ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "M_EC_STR", ecCode, ParameterDirection.InputOutput, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_POINT_STR", psCode, ParameterDirection.InputOutput, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_COUNT_STR", "", ParameterDirection.InputOutput, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_NG_NUM", "", ParameterDirection.InputOutput, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "M_LINK_NUM", null, ParameterDirection.Output, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "FLOWCODE", null, ParameterDirection.Output, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_MARK", null, ParameterDirection.Output, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_MO_NUMBER", null, ParameterDirection.Output, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "RES", null, ParameterDirection.Output, valueTypes.STRING);

            return dbHelper.htExecuteNonQuery(ListDBParameters, "P_LK_CHECK_SAVE_EC");
        }
        #endregion

        #region dgvFill
        /// <summary>
        /// dataGridView表格填充
        /// </summary>
        /// <param name="sn">SN</param>
        /// <param name="type">良品/不良品</param>
        /// <param name="status">OK/NG</param>
        /// <param name="msg">msg</param>
        private void dgvFill(string sn, string type, string status, string msg)
        {
            if (dataGridView1.Rows.Count >= 80)
                dataGridView1.Rows.Clear();

            if ("不良品".Equals(type) && "OK".Equals(status))
                msg = "产品SN未过站成功";
            if (dataGridView1.Rows.Count >= 100)
            {
                dataGridView1.Rows.Clear();
            }
            int index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells["SN"].Value = sn;
            dataGridView1.Rows[index].Cells["Time"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataGridView1.Rows[index].Cells["Type"].Value = type;
            dataGridView1.Rows[index].Cells["Status"].Value = status;
            dataGridView1.Rows[index].Cells["Error"].Value = msg;
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);
        }
        #endregion

        #region 日志
        /// <summary>
        /// info日志记录
        /// </summary>
        /// <param name="sn">条码SN</param>
        /// <param name="ecCode">不良代码</param>
        /// <param name="ht">存储返回集合</param>
        private void sbLogs(string sn, string ecCode, Hashtable ht)
        {
            StringBuilder sbinfo = new StringBuilder();
            try
            {
                sbinfo.Append("\r\n{MC=P_C_CHECK_EC,Parameter:[{M_DATA_AUTH=").Append(data_auth).Append(",M_SN=").Append(sn);
                sbinfo.Append(",M_WORK_STATIONID=").Append(WorkStation).Append(",M_EMP=").Append(m_UsrCode).Append(",M_FLAG=");
                sbinfo.Append(",M_EC_STR=").Append(ecCode);

                sbinfo.Append(",M_POINT_STR=").Append(ht["M_POINT_STR"].ToString()).Append(",M_COUNT_STR=").Append(ht["M_COUNT_STR"].ToString());
                sbinfo.Append(",M_NG_NUM=").Append(ht["M_NG_NUM"].ToString()).Append(",M_LINK_NUM=").Append(ht["M_LINK_NUM"].ToString());
                sbinfo.Append(",FLOWCODE=").Append(ht["FLOWCODE"].ToString()).Append(",M_MARK=").Append(ht["M_MARK"].ToString());
                sbinfo.Append(",M_MO_NUMBER=").Append(ht["M_MO_NUMBER"].ToString()).Append(",RES=").Append(ht["RES"].ToString()).Append("}]");
                sbinfo.Append("}");
            }
            catch (Exception ex)
            {
                sbinfo.Append("{M_SN=").Append(sn).Append(",M_EC_STR=").Append(ecCode);
                sbinfo.Append(",MSG=INFO记录异常,").Append("EXP").Append(ex.ToString()).Append("}");
            }
            SimpleLoger.Instance.Info(sbinfo.ToString());
        }
        #endregion

        #region CScanResult ExecScanBarOperate
        public CScanResult ExecScanBarOperate(string BarString)
        {
            CScanResult csr = new CScanResult();
            csr.BarString = BarString;
            csr.Result = "OK";
            csr.Remark = "";
            return csr;
        }
        #endregion

        #region SPI
        private bool readSpiData()
        {

            return true;
        }
        #endregion

        #region AOI
        #region 德律
        private bool readAoiDlData()
        {
            return true;
        }
        #endregion
        #region 劲拓  单PCS输出一份txt文档
        private bool readAoiJtData()
        {
         
            return true;
        }
        #endregion
        #endregion
    }
}
