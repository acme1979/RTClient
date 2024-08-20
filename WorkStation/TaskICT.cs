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
using System.Threading;
using System.Text.RegularExpressions;

/**
 * 菱电ICT过站解析
 * 读取scv
 */ 
namespace WorkStation
{
    public partial class TaskICT : CUserControl, ICommonWorkStationCtl
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

        public TaskICT()
        {
            InitializeComponent();
            tbReadPath.Click += new EventHandler(tbReadPath_Click);
            tbBackPath.Click += new EventHandler(tbBackPath_Click);

            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);

            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);

            cbType.SelectedIndexChanged += new EventHandler(cbType_SelectedIndexChanged);
        }


        #region InitData
        public void InitData()
        {
            tbReadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath", "", wsIniPath);
            tbBackPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BackPath", "", wsIniPath);
            tbReadRate.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Fre", "", wsIniPath);

            ip = Computer.Instance().IpAddress;

            string sql = "SELECT CODE,VALUE FROM SY_DICT_VAL WHERE DICT_CODE = 'DeviceICTType' ORDER BY DISP_INDX";
            DataTable dt = dbHelper.GetDataTable(sql, "SY_DICT_VAL");
            this.cbType.DataSource = dt;
            cbType.DisplayMember = "VALUE";
            cbType.ValueMember = "CODE";

            string cValue = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "DevType", "", wsIniPath);
            if (cValue.Length > 0)
            {
                cbType.SelectedValue = cValue;
            }
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
                string cValue = cbType.SelectedValue.ToString();
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ReadPath" + cValue, this.tbReadPath.Text, wsIniPath);
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
                string cValue = cbType.SelectedValue.ToString();
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "BackPath" + cValue, this.tbBackPath.Text, wsIniPath);
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
            switch(cbType.SelectedValue.ToString())
            {
                case "1":
                    isTrue = MachineDataRead(); //老ICT设备
                    break;
                case "2":
                    isTrue = NewIctDataRead(); //新ICT设备
                    break;
            }

            if (!isTrue)
                btnEnd_Click(null, null);
            timer2.Enabled = isTrue;
        }
        #endregion


        #region  业务处理 MachineDataRead()
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
                if (lines.Length < 3)
                    continue;
                #endregion

                //文本未被占用,读取文本,
                string TestFileName = lines[1].Split(',')[0].Trim();
                string TestRes = lines[1].Split(',')[1].Trim();
                string snCode = lines[1].Split(',')[2].Trim();
                string TestTime = lines[1].Split(',')[3].Trim();
                string TestEmp = lines[1].Split(',')[4].Trim();
                ecCode = "";
                string ngNum = "";
                if (!"PASS".Equals(TestRes.ToUpper()))
                {
                    ecCode = "|ICT001";
                    ngNum = "1";
                }

                string res = "";
                string wis = "";
                if (ckIsGZ.Checked && !"PASS".Equals(TestRes.ToUpper()))
                {
                    res = "NG:SN不过站!";
                    wis = "OK";
                }
                else
                {
                    ht = run(snCode, ecCode, ngNum);
                    res = ht["RES"].ToString();
                    wis = res.Substring(0, 2);
                }
                ht.Clear();
                if (ckTcMsg.Checked && "NG".Equals(wis))
                {
                    CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                }

                DataTable dt = dtmx(lines);

                #region 数据保存
                    string guid = Guid.NewGuid().ToString("N");
                    StringBuilder sb = new StringBuilder("");
                    sb.Append("INSERT INTO T_WIP_ICT (ID, CREATE_USER, CREATE_TIME, DATA_AUTH,");
                    sb.Append("TEST_FILE_NAME, TEST_RESULT, TEST_SN, TEST_MES_RES, TEST_MES_INFO)VALUES (");
                    sb.Append("'" + guid + "','" + TestEmp + "',to_date('" + TestTime + "','yyyy-mm-dd hh24:mi:ss'),'" + data_auth + "','");
                    sb.Append(TestFileName + "','" + TestRes + "','" + snCode + "','" + wis + "','" + res + "')");
                    try
                    {
                        dbHelper.ExecuteSql(sb.ToString());
                    }
                    catch (Exception err)
                    {
                        SimpleLoger.Instance.Error("\r\n " + "老化主数据保存失败:" + err.ToString() + "\r\n " + sb.ToString());
                        dgvFill(snCode, TestRes, "NG", "主数据保存失败");
                        continue;
                    }

                if (!"PASS".Equals(TestRes.ToUpper()))
                {
                    sb.Clear();
                    sb.Append("BEGIN ");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("INSERT INTO T_WIP_ICT_DETAIL(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ICT_ID ");
                        StringBuilder sbName = new StringBuilder();
                        StringBuilder sbValue = new StringBuilder();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string columnName = dt.Columns[j].ColumnName;
                            string ColumnValue = dt.Rows[i]["" + columnName + ""].ToString();
                            sbName.Append("," + columnName);
                            sbValue.Append(",'" + ColumnValue + "'");
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
                        SimpleLoger.Instance.Error("\r\n " + "数据明细保存失败:" + err.ToString() + "\r\n " + sb.ToString());
                        dgvFill(snCode, TestRes, "NG", "明细数据保存失败");
                        continue;
                    }
                }
                
                #endregion


                #region 文件名称变更
                string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                File.Move(file, newPath);
                #endregion

                dgvFill(snCode, TestRes, wis, res);
                isBreak++;
            }
            DBHelper.Instance.FlashDBObject(DBCode);
            return true;
        }
        #endregion

        #region 新ICT设备业务处理
        private bool NewIctDataRead()
        {
            List<string> files = new List<string>();
            files = CWorkFlowControlHelper.GetDirFiles(tbReadPath.Text, files, "*.txt");
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
                if (lines.Length < 1)
                    continue;
                #endregion

                string rData = lines[0].Trim();
                string snCode = rData.Substring(0, rData.IndexOf(@"_"));
                string TestRes = rData.Substring(rData.IndexOf(@"_") + 1);
                ecCode = "";
                string ngNum = "";
                if (!"PASS".Equals(TestRes.ToUpper()))
                {
                    ecCode = "|ICT001";
                    ngNum = "1";
                }

                string res = "";
                string wis = "";
                if (ckIsGZ.Checked && !"PASS".Equals(TestRes.ToUpper()))
                {
                    res = "NG:SN不过站!";
                    wis = "OK";
                }
                else
                {
                    ht = run(snCode, ecCode, ngNum);
                    res = ht["RES"].ToString();
                    wis = res.Substring(0, 2);
                }
                ht.Clear();
                if (ckTcMsg.Checked && "NG".Equals(wis))
                {
                    CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                }
                #region 文件名称变更
                string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                File.Move(file, newPath);
                #endregion

                dgvFill(snCode, TestRes, wis, res);
                isBreak++;
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
        /// <param name="ht"></param>
        /// <returns></returns>
        private Hashtable run(string SN, string ecCode, string ngNum)
        {
            List<DBParameters> ListDBParameters = new List<DBParameters>();
            Hashtable ht = new Hashtable();
            Hashtable htDmp = new Hashtable();

            dbHelper.AddDBParameters(ListDBParameters, "M_DATA_AUTH", data_auth, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_SN", SN, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_WORK_STATIONID", WorkStation, ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "M_EC_STR", ecCode, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_POINT_STR", "", ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_COUNT_STR", "", ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_NG_NUM", ngNum, ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "M_EMP", m_UsrCode, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_FLAG", "", ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "FLOWCODE", null, ParameterDirection.Output, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "RES", null, ParameterDirection.Output, valueTypes.STRING);

            htDmp.Add("M_DATA_AUTH", data_auth);
            htDmp.Add("M_SN", SN);
            htDmp.Add("M_WORK_STATIONID", WorkStation);
            htDmp.Add("M_EC_STR", ecCode);
            htDmp.Add("M_POINT_STR", "");
            htDmp.Add("M_COUNT_STR", "");
            htDmp.Add("M_NG_NUM", ngNum);
            htDmp.Add("M_EMP", m_UsrCode);
            htDmp.Add("M_FLAG", "");

            string beginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                ht = dbHelper.htExecuteNonQuery(ListDBParameters, "P_C_SAVE_SN_TEST2");
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
                string tName = "[" + ip + "]:ICT " + SN;
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
        /// <param name="bStatus">文件条码结果</param>
        /// <param name="Status">过站结果</param>
        /// <param name="msg">过站信息</param>
        private void dgvFill(string SN, string bStatus, string Status, string msg)
        {
            if (dataGridView1.Rows.Count >= 80)
                dataGridView1.Rows.Clear();

            int index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells["SN"].Value = SN;
            dataGridView1.Rows[index].Cells["Time"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataGridView1.Rows[index].Cells["bStatus"].Value = bStatus;
            dataGridView1.Rows[index].Cells["Status"].Value = Status;
            dataGridView1.Rows[index].Cells["Msg"].Value = msg;
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);
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
            dt.Columns.Add(new DataColumn("STEP_NO", typeof(string)));
            dt.Columns.Add(new DataColumn("CELL_NAME", typeof(string)));
            dt.Columns.Add(new DataColumn("TM", typeof(string)));
            dt.Columns.Add(new DataColumn("TYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("AREA_NO", typeof(string)));
            dt.Columns.Add(new DataColumn("NEEDLE1", typeof(string)));
            dt.Columns.Add(new DataColumn("NEEDLE2", typeof(string)));
            dt.Columns.Add(new DataColumn("NEEDLE3", typeof(string)));
            dt.Columns.Add(new DataColumn("NEEDLE4", typeof(string)));

            dt.Columns.Add(new DataColumn("CALL_STA", typeof(string)));
            dt.Columns.Add(new DataColumn("STA1", typeof(string)));
            dt.Columns.Add(new DataColumn("ADD1", typeof(string)));
            dt.Columns.Add(new DataColumn("SUB1", typeof(string)));
            dt.Columns.Add(new DataColumn("MEASURE1", typeof(string)));
            dt.Columns.Add(new DataColumn("ERROR1", typeof(string)));
            dt.Columns.Add(new DataColumn("DELAYED", typeof(string)));
            dt.Columns.Add(new DataColumn("STA2", typeof(string)));
            dt.Columns.Add(new DataColumn("ADD2", typeof(string)));
            dt.Columns.Add(new DataColumn("SUB2", typeof(string)));
            dt.Columns.Add(new DataColumn("MEASURE2", typeof(string)));
            dt.Columns.Add(new DataColumn("ERROR2", typeof(string)));

            dt.Columns.Add(new DataColumn("G1", typeof(string)));
            dt.Columns.Add(new DataColumn("G2", typeof(string)));
            dt.Columns.Add(new DataColumn("G3", typeof(string)));
            dt.Columns.Add(new DataColumn("G4", typeof(string)));
            dt.Columns.Add(new DataColumn("G5", typeof(string)));
            dt.Columns.Add(new DataColumn("G6", typeof(string)));
            dt.Columns.Add(new DataColumn("G7", typeof(string)));

            dt.Columns.Add(new DataColumn("TEST_RES", typeof(string)));
            #endregion

            DataRow dr = null;
            for (int i = 6; i < lines.Length; i++)
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

        #region cbType_SelectedIndexChanged
        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cValue = cbType.SelectedValue.ToString();
            if ("System.Data.DataRowView".Equals(cValue))
                return;
            IniFileHelper.WriteIniData("workStationNum", WorkStation + "DevType", cValue, wsIniPath);
            tbReadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath" + cValue, "", wsIniPath);
            tbBackPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BackPath" + cValue, "", wsIniPath);
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

        #region 按日期生成文件夹
        private string CreatFolderByDate(string path)
        {
            string datestr = System.DateTime.Now.ToString("yyyyMMdd");
            string basepath = path;
            string backpath = basepath + @"\" + datestr;
            if (Directory.Exists(backpath))
            {
                return backpath;
            }
            else
            {
                Directory.CreateDirectory(backpath);
                return backpath;
            }
        }
        #endregion

    }
}
