using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseModel;
using System.Collections;
using WorkStation.FunClass;
using System.IO;
using BaseModel.Logs;
using System.Text.RegularExpressions;

/**
 * 老化及台架测试
 * 读取文本文件  一次性抛出一个csv
 *  抓取不过站  过站调用MES接口
 */
namespace WorkStation
{
    public partial class TaskBurnIn : CUserControl, ICommonWorkStationCtl
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
        public TaskBurnIn()
        {
            InitializeComponent();
            tbReadPath.Click += new EventHandler(tbReadPath_Click);
            tbBackPath.Click += new EventHandler(tbBackPath_Click);

            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);

            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);
        }

        #region InitData
        public void InitData()
        {
            tbReadRate.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Fre", "", wsIniPath);

            ip = Computer.Instance().IpAddress;
            tbReadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath", "", wsIniPath);
            tbBackPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BackPath", "", wsIniPath);
        }
        #endregion

        #region 读备文件设置
        private void tbReadPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个文件夹";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbReadPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ReadPath", this.tbReadPath.Text, wsIniPath);
            }
        }
        private void tbBackPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个备份文件夹";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbBackPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "BackPath", this.tbBackPath.Text, wsIniPath);
            }
        }
        #endregion

        #region btnStart_Click
        private void btnStart_Click(object sender, EventArgs e)
        {
            if ("".Equals(tbReadPath.Text))
            {
                MessageBox.Show("未设置读取目录！");
                return;
            }
            if ("".Equals(tbBackPath.Text))
            {
                MessageBox.Show("未设置备份目录！");
                return;
            }

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

            try
            {
                DBHelper.Instance.FlashDBObject(DBCode);
            }
            catch (Exception)
            { }
            dbHelper = DBHelper.Instance.GetDBInstace(DBCode);
        }
        #endregion

        #region SetEnable()
        private void SetEnable()
        {
            tbReadPath.Enabled = !tbReadPath.Enabled;
            tbBackPath.Enabled = !tbBackPath.Enabled;
            btnEnd.Enabled = !btnEnd.Enabled;
            btnStart.Enabled = !btnStart.Enabled;
            tbReadRate.ReadOnly = !tbReadRate.ReadOnly;
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
            bool isTrue = false;
            isTrue = MachineDataRead();
            timer2.Enabled = isTrue;
            if (!isTrue)
                btnEnd_Click(null, null);
        }
        #endregion

        #region 业务处理
        private bool MachineDataRead()
        {
            List<string> files = new List<string>();
            files = CWorkFlowControlHelper.GetDirFiles(tbReadPath.Text, files, "*.csv");
            if (files.Count <= 0)
                return true;

            dbHelper = DBHelper.Instance.GetDBInstace(DBCode);
            string ecCode = "";
            Hashtable ht = new Hashtable();
            int isBreak = 0;
            foreach (string file in files)
            {
                if (isBreak > 20)
                {
                    break;
                }
                string[] lines = null;
                string snCode = "";
                try
                {

                    #region 文件是否被占用
                    try
                    {
                        lines = File.ReadAllLines(file, System.Text.Encoding.Default);
                    }
                    catch (Exception ex)
                    {
                        SimpleLoger.Instance.Error("\r\n " + "文件被占用:" + file + "\r\n ");
                        continue;
                    }
                    if (lines.Length < 8)
                        continue;
                    #endregion

                    //文本未被占用,读取文本,
                    string station = lines[0].Split(',')[1];  //PN
                    string _operator = lines[1].Split(',')[1];
                    string device = lines[2].Split(',')[1];
                    string pn = lines[3].Split(',')[1];
                    snCode = lines[4].Split(',')[1];
                    string result = lines[5].Split(',')[1];
                    string start_time = lines[6].Split(',')[1];
                    DateTime dts;
                    try
                    {
                        DateTime.TryParse(start_time, out dts);
                    }
                    catch (Exception)
                    {
                        start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    string end_time = lines[7].Split(',')[1];
                    try
                    {
                        DateTime.TryParse(start_time, out dts);
                    }
                    catch (Exception)
                    {
                        end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    DataTable dt = dtmx(lines);

                    #region 数据保存
                    string guid = Guid.NewGuid().ToString("N");
                    StringBuilder sb = new StringBuilder("");
                    sb.Append("INSERT INTO T_WIP_BURN_IN (ID, CREATE_USER, CREATE_TIME, DATA_AUTH,");
                    sb.Append("BI_STATION, BI_OPERATOR, BI_DEVICE, BI_PN, BI_SN, BI_RESULT, BI_START_TIME, BI_END_TIME)VALUES (");
                    sb.Append("'" + guid + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','" + station + "','" + _operator + "','");
                    sb.Append(device + "','" + pn + "','" + snCode + "','" + result + "',to_date('" + start_time + "','yyyy-mm-dd hh24:mi:ss'),");
                    sb.Append("to_date('" + end_time + "','yyyy-mm-dd hh24:mi:ss'))");
                    try
                    {
                        dbHelper.ExecuteSql(sb.ToString());
                    }
                    catch (Exception err)
                    {
                        CWorkFlowControlHelper.BackUpFile(tbBackPath.Text + "\\errorFile", file);
                        SimpleLoger.Instance.Error("\r\n " + "老化主数据保存失败:" + err.ToString() + "\r\n " + sb.ToString());
                        dgvFill(snCode, "NG", "主数据保存失败");
                        continue;
                    }

                    sb.Clear();
                    sb.Append("BEGIN ");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("INSERT INTO T_WIP_BURN_IN_DETAIL(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, BD_ID ");

                        StringBuilder sbName = new StringBuilder();
                        StringBuilder sbValue = new StringBuilder();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string columnName = dt.Columns[j].ColumnName;
                            string ColumnValue = dt.Rows[i]["" + columnName + ""].ToString();

                            if ("DB_TIME".Equals(columnName))
                            {
                                sbName.Append("," + columnName);
                                if (ColumnValue.Length <= 15)
                                    ColumnValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                sbValue.Append(",to_date('" + ColumnValue + "','yyyy-mm-dd hh24:mi:ss')");
                            }
                            else
                            {
                                sbName.Append("," + columnName);
                                sbValue.Append(",'" + ColumnValue + "'");
                            }
                        }

                        sb.Append(sbName + ") VALUES");
                        sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','" + guid + "'");
                        sb.Append(sbValue + ");");
                    }
                    sb.Append("END;");
                    try
                    {
                        if (sb.Length > 20)
                            dbHelper.ExecuteSql(sb.ToString());
                    }
                    catch (Exception err)
                    {
                        #region 文件名称变更
                        //string errFile = tbBackPath.Text + "\\errorFile";
                        //if (!Directory.Exists(errFile))
                        //{
                        //    Directory.CreateDirectory(errFile);
                        //}
                        //string errorPath = Path.Combine(errFile, DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                        //File.Move(file, errorPath);
                        CWorkFlowControlHelper.BackUpFile(tbBackPath.Text + "\\errorFile", file);
                        #endregion

                        SimpleLoger.Instance.Error("\r\n " + "数据明细保存失败:" + err.ToString() + "\r\n " + sb.ToString());
                        dgvFill(snCode, "NG", "明细数据保存失败");
                        continue;
                    }
                    #endregion

                    #region 文件名称变更
                    //string newPath = Path.Combine(tbBackPath.Text, DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                    //File.Move(file, newPath);
                    CWorkFlowControlHelper.BackUpFile(tbBackPath.Text, file);
                    #endregion
                }
                catch (Exception exc)
                {
                    CWorkFlowControlHelper.BackUpFile(tbBackPath.Text + "\\errorFile", file);
                    dgvFill(snCode, "NG", "数据解析失败");
                    continue;
                }
                dgvFill(snCode, "OK", "数据保存成功");
                isBreak++;
            }
            DBHelper.Instance.FlashDBObject(DBCode);
            return true;
        }
        #endregion

        #region 获取明细
        /// <summary>
        /// 老化明细数据
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private DataTable dtmx(string[] lines)
        {
            DataTable dt = new DataTable("mx");

            #region 列名
            dt.Columns.Add(new DataColumn("DB_TIME", typeof(string)));
            dt.Columns.Add(new DataColumn("BD_INPUT", typeof(string)));
            dt.Columns.Add(new DataColumn("BD_TEMPERATURE", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_WATER_TEMPERATURE", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_WATER_GAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_FLOW", typeof(string)));

            dt.Columns.Add(new DataColumn("DB_INPUT_HVV", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_HVV_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_HVV_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_INPUT_HVA", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_HVA_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_HVA_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_INPUT_LVV", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_LVV_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_LVV_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_INPUT_LVA", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_LVA_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_LVA_SX", typeof(string)));

            dt.Columns.Add(new DataColumn("DB_AV", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_BV", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_CV", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_V_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_V_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AA", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_BA", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_CA", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_A_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_A_SX", typeof(string)));

            dt.Columns.Add(new DataColumn("DB_BUSBAR_VOLTAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_BV_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_BV_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_BUSBAR_ELECTRICITY", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_BE_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_BE_SX", typeof(string)));

            dt.Columns.Add(new DataColumn("DB_AMT", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AMT_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AMT_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AMS", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AMS_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AMS_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_IS", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IS_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IS_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_ID_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_ID_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_IQ", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IQ_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IQ_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_US", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_US_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_US_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_UD", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_UD_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_UD_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_UQ", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_UQ_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_UQ_SX", typeof(string)));

            dt.Columns.Add(new DataColumn("DB_AMT_ORDER", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AT_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AT_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AMS_ORDER", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AS_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_AS_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_IGBT", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IGBT_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IGBT_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_MMT1", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MMT1_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MMT1_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MCU_MMT2", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MMT2_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MMT2_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MPM", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MPM_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MPM_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IPM", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IPM_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IPM_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_HALL", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_HALL_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_HALL_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_U_VOL", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_U_VOL_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_U_VOL_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_V_VOL", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_V_VOL_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_V_VOL_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_W_VOL", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_W_VOL_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_W_VOL_SX", typeof(string)));

            dt.Columns.Add(new DataColumn("DB_FOC", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_FOC_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_FOC_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_SYS_TIMER", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_ST_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_ST_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_PWM_HARD", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_PH_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_PH_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_PWM_SOFT", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_PS_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_PS_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MLF", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MLF_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MLF_SX", typeof(string)));

            dt.Columns.Add(new DataColumn("DB_MDF", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MDF_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MDF_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_KS1", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_KS1_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_KS1_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_KS2", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_KS2_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_KS2_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IGBT", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IGBT_S_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_IGBT_S_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_RDC", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_RDC_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_RDC_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_ISS", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_ISS_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_ISS_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MSC", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MSC_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MSC_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MTC", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MTC_XX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_MTC_SX", typeof(string)));
            dt.Columns.Add(new DataColumn("DB_RES", typeof(string)));
            #endregion

            DataRow dr = null;
            for (int i = 11; i < lines.Length; i++)
            {
                string sa = lines[i].ToString();
                string[] ssa = Regex.Split(sa, ",");
                dr = dt.NewRow();
                for (int j = 0; j < ssa.Length; j++)
                {
                    string ColumnValue = ssa[j];
                    string columnName = dt.Columns[j].ColumnName;
                    dr["" + columnName + ""] = ColumnValue;

                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region 生产测试指令
        /// <summary>
        /// 生产测试指令
        /// </summary>
        /// <param name="ListDBParameters"></param>
        /// <param name="SN">条码SN</param>
        /// <param name="ecCode">不良代码</param>
        /// <param name="ecCode">不良点位</param>
        /// <param name="csCount">不良点数</param>
        /// <param name="ht"></param>
        /// <returns></returns>
        private Hashtable run(string SN, string ecCode, string ngNum, string psCode, string csCount)
        {
            List<DBParameters> ListDBParameters = new List<DBParameters>();
            Hashtable ht = new Hashtable();
            Hashtable htDmp = new Hashtable();

            dbHelper.AddDBParameters(ListDBParameters, "M_DATA_AUTH", data_auth, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_SN", SN, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_WORK_STATIONID", WorkStation, ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "M_EC_STR", ecCode, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_POINT_STR", psCode, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_COUNT_STR", csCount, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_NG_NUM", ngNum, ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "M_EMP", m_UsrCode, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_FLAG", "", ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "FLOWCODE", null, ParameterDirection.Output, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "RES", null, ParameterDirection.Output, valueTypes.STRING);

            htDmp.Add("M_DATA_AUTH", data_auth);
            htDmp.Add("M_SN", SN);
            htDmp.Add("M_WORK_STATIONID", WorkStation);
            htDmp.Add("M_EC_STR", ecCode);
            htDmp.Add("M_POINT_STR", psCode);
            htDmp.Add("M_COUNT_STR", csCount);
            htDmp.Add("M_NG_NUM", ngNum);
            htDmp.Add("M_EMP", m_UsrCode);
            htDmp.Add("M_FLAG", "");

            string beginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                ht = dbHelper.htExecuteNonQuery(ListDBParameters, "P_C_SAVE_SN_TEST");
            }
            catch (Exception err)
            {
                htDmp.Add("RES", "NG:存储执行异常！");
                ht.Add("RES", "NG:存储执行异常！");
            }
            htDmp.Add("FLOWCODE", ht["FLOWCODE"]);
            htDmp.Add("RES", ht["RES"]);
            string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //指令运行日志
            try
            {
                StringBuilder sb = new StringBuilder("BEGIN ");
                string tid = Guid.NewGuid().ToString();
                string tName = "[" + ip + "]:SPI " + SN;
                sb.Append("INSERT INTO T_FW_TASK_LOG (TFTL_ID,TFTL_NAME,TFTL_PROCE_DATE,TFTL_PROCE_EN_DATE,TFTL_DMP_ID,TFTL_IN_DATE)VALUES ('");
                sb.Append(tid + "','" + tName + "','" + beginTime + "','" + endTime + "','VOL_DMP','" + beginTime + "');");

                sb.Append("INSERT INTO T_FW_TASK_PROC_LOG(TFTL_ID, TFTRL_NAME, TFTRL_ST_DATE, TFTRL_EN_DATE)VALUES ('");
                sb.Append(tid + "','P_C_SAVE_SN_TEST','" + beginTime + "','" + endTime + "');");

                foreach (string key in htDmp.Keys)
                {
                    string type = "IN";
                    if ("RES".Equals(key))
                        type = "OUT";
                    sb.Append("INSERT INTO T_FW_TASK_PROC_PARA_LOG(TFTL_ID, TFTRL_NAME, TFTPL_NAME, TFTPL_VALUE, TFTPL_TYPE, TFTL_DATE)VALUES ('");
                    sb.Append(tid + "','P_C_SAVE_SN_TEST','" + key + "','" + htDmp[key] + "','" + type + "',SYSDATE);");
                }

                sb.Append("END;");
                string sa = sb.ToString();
                dbHelper.ExecuteSql(sb.ToString());
            }
            catch { }

            return ht;
        }
        #endregion

        #region dgvFill
        /// <summary>
        /// dataGridView表格填充
        /// </summary>
        /// <param name="SN">条码</param>
        /// <param name="Status">过站结果</param>
        /// <param name="msg">过站信息</param>
        private void dgvFill(string SN, string Status, string msg)
        {
            if (dataGridView1.Rows.Count >= 80)
                dataGridView1.Rows.Clear();

            int index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells["SN"].Value = SN;
            dataGridView1.Rows[index].Cells["Time"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataGridView1.Rows[index].Cells["Status"].Value = Status;
            dataGridView1.Rows[index].Cells["Msg"].Value = msg;
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);
        }
        #endregion

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


    }
}
