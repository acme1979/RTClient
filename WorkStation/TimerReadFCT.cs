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
using BaseModel.Logs;
using System.IO;
using WorkStation.FunClass;
using System.Text.RegularExpressions;

/**
 *  菱电GCU自动线FCT
 *   解析  csv后缀的txt文本格式
 */
namespace WorkStation
{
    public partial class TimerReadFCT : CUserControl, ICommonWorkStationCtl
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
        public TimerReadFCT()
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
            foreach (string file in files)
            {
                string[] lines = null;
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
                if (lines.Length < 7)
                    continue;
                #endregion

                //文本未被占用,读取文本,
                string pn = Regex.Split(lines[0].Trim(), @"\t")[1].Replace("\"", string.Empty);  //PN
                string snCode = Regex.Split(lines[1].Trim(), @"\t")[1].Replace("\"", string.Empty); // lines[1].Trim();
                string time = Regex.Split(lines[2].Trim(), @"\t")[1].Replace("\"", string.Empty);  //lines[2].Trim();
                string tester = Regex.Split(lines[3].Trim(), @"\t")[1].Replace("\"", string.Empty);  //lines[3].Trim();
                string testProg = Regex.Split(lines[4].Trim(), @"\t")[1].Replace("\"", string.Empty); // lines[4].Trim();
                string testProgRev = Regex.Split(lines[5].Trim(), @"\t")[1].Replace("\"", string.Empty); // lines[5].Trim();
                string result = Regex.Split(lines[6].Trim(), @"\t")[1].Replace("\"", string.Empty); // lines[6].Trim();
                ecCode = "";
                string ngNum = "";
                string errDw = "";
                if (!"OK".Equals(result.ToUpper()))
                {
                    ecCode = "|FCT001";
                    ngNum = "1";
                }
                #region 获取明细
                DataTable dt = new DataTable("mx");
                dt.Columns.Add(new DataColumn("GCU_FCT_STEP", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_STEPNAME", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_NAME", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_VALUE", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_UNIT", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_STATUS", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_LOWLMT", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_UPLMT", typeof(string)));
                dt.Columns.Add(new DataColumn("GCU_FCT_DESP", typeof(string)));
                DataRow dr = null;
                for (int i = 9; i < lines.Length; i++)
                {
                    string sa = lines[i].ToString();
                    string[] ssa = Regex.Split(sa, @"\t");
                    dr = dt.NewRow();
                    dr["GCU_FCT_STEP"] = ssa[0].Replace("\"", string.Empty);
                    dr["GCU_FCT_STEPNAME"] = ssa[1].Replace("\"", string.Empty);
                    dr["GCU_FCT_NAME"] = ssa[2].Replace("\"", string.Empty);
                    dr["GCU_FCT_VALUE"] = ssa[3].Replace("\"", string.Empty);
                    dr["GCU_FCT_UNIT"] = ssa[4].Replace("\"", string.Empty);
                    dr["GCU_FCT_STATUS"] = ssa[5].Replace("\"", string.Empty);
                    dr["GCU_FCT_LOWLMT"] = ssa[6].Replace("\"", string.Empty);
                    dr["GCU_FCT_UPLMT"] = ssa[7].Replace("\"", string.Empty);
                    dr["GCU_FCT_DESP"] = ssa[8].Replace("\"", string.Empty);
                    dt.Rows.Add(dr);
                }
                #endregion

                //ht = run(snCode, ecCode, ngNum, errDw, "");
                //string res = ht["RES"].ToString();
                //string wis = res.Substring(0, 2);
                //ht.Clear();
                //if (ckTcMsg.Checked && "NG".Equals(wis))
                //{
                //    CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                //}

                string res = "";
                string wis = "";
                #region 数据保存
                string guid = Guid.NewGuid().ToString("N");
                StringBuilder sb = new StringBuilder("");
                time = time.Replace('T', ' ');
                sb.Append("INSERT INTO T_WIP_GCU_FCT (ID, CREATE_USER, CREATE_TIME, DATA_AUTH,GCU_FCT_PN, GCU_FCT_SN,GCU_FCT_TIME,");
                sb.Append("GCU_FCT_TESTER, GCU_FCT_TESTPROG,GCU_FCT_TESTPROGREV, GCU_FCT_RESULT, GCU_MES_RES, GCU_MES_MSG)VALUES (");
                sb.Append("'" + guid + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','" + pn + "','" + snCode + "',");
                sb.Append("to_date('" + time + "','yyyy-mm-dd hh24:mi:ss'),'" + tester + "','" + testProg + "','");
                sb.Append(testProgRev + "','" + result + "','" + wis + "','" + res + "')");
                try
                {
                    dbHelper.ExecuteSql(sb.ToString());
                }
                catch (Exception err)
                {
                    SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sb.ToString());
                }

                //StringBuilder sb = new StringBuilder("BEGIN ");
                sb.Clear();
                sb.Append("BEGIN ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("INSERT INTO T_WIP_GCU_FCT_DETAIL(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ");
                    sb.Append("GCU_FCT_STEP, GCU_FCT_STEPNAME, GCU_FCT_NAME, GCU_FCT_VALUE, GCU_FCT_UNIT, ");
                    sb.Append("GCU_FCT_STATUS,GCU_FCT_LOWLMT, GCU_FCT_UPLMT, GCU_FCT_DESP, GCU_ITEM_ID)VALUES ");
                    sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                    sb.Append(dt.Rows[i]["GCU_FCT_STEP"] + "','" + dt.Rows[i]["GCU_FCT_STEPNAME"] + "','" + dt.Rows[i]["GCU_FCT_NAME"] + "','");
                    sb.Append(dt.Rows[i]["GCU_FCT_VALUE"] + "','" + dt.Rows[i]["GCU_FCT_UNIT"] + "','" + dt.Rows[i]["GCU_FCT_STATUS"] + "','");
                    sb.Append(dt.Rows[i]["GCU_FCT_LOWLMT"] + "','" + dt.Rows[i]["GCU_FCT_UPLMT"] + "','" + dt.Rows[i]["GCU_FCT_DESP"] + "','" + guid + "'); ");
                }
                sb.Append("END;");
                try
                {
                    if (sb.Length > 20)
                        dbHelper.ExecuteSql(sb.ToString());
                }
                catch (Exception err)
                {
                    SimpleLoger.Instance.Error("\r\n " + "数据明细保存失败:" + err.ToString() + "\r\n " + sb.ToString());
                }
                #endregion

                #region 文件名称变更
                string newPath = Path.Combine(tbBackPath.Text, DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                File.Move(file, newPath);
                #endregion

                dgvFill(snCode, wis, res);
            }
            DBHelper.Instance.FlashDBObject(DBCode);
            return true;
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
