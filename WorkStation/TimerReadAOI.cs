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
using WorkStation.FunClass;
using BaseModel.Logs;
using System.IO;

/**
 * 菱电AOI
 *    伍洲 txt格式、雅马哈 txt格式
 * 
 */
namespace WorkStation
{
    public partial class TimerReadAOI : CUserControl, ICommonWorkStationCtl
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

        public TimerReadAOI()
        {
            InitializeComponent();
            tbReadPath.Click += new EventHandler(tbReadPath_Click);
            tbBackPath.Click += new EventHandler(tbBackPath_Click);

            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);

            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);

            cbType.SelectedIndexChanged += new EventHandler(cbType_SelectedIndexChanged);
            ckTcMsg.CheckedChanged += new EventHandler(ckTcMsg_CheckedChanged);
            btnFPYQuerry.Click += new EventHandler(btnFPYQuerry_Click);
        }

        void ckTcMsg_CheckedChanged(object sender, EventArgs e)
        {
            string val = ckTcMsg.Checked.ToString();
            IniFileHelper.WriteIniData("workStationNum", WorkStation + "ckTcMsg", val, wsIniPath);
        }

        #region btnFPYQuerry_Click
        private void btnFPYQuerry_Click(object sender, EventArgs e)
        {
            FPYQuerry fpy = new FPYQuerry();
            fpy.FPY_DATA_AUTH = this.data_auth;
            fpy.FPY_DBHelper = this.dbHelper;
            fpy.Show();
        }
        #endregion

        #region InitData
        public void InitData()
        {
            tbReadRate.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Fre", "", wsIniPath);
            tbIP.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "IP", "", wsIniPath);

            ip = Computer.Instance().IpAddress;

            string sql = "SELECT CODE,VALUE FROM SY_DICT_VAL WHERE DICT_CODE = 'DeviceAOIType' ORDER BY DISP_INDX";
            DataTable dt = dbHelper.GetDataTable(sql, "SY_DICT_VAL");
            this.cbType.DataSource = dt;
            cbType.DisplayMember = "VALUE";
            cbType.ValueMember = "CODE";

            string cValue = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "DevType", "", wsIniPath);
            if (cValue.Length > 0)
            {
                cbType.SelectedValue = cValue;
                tbReadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath" + cValue, "", wsIniPath);
                tbBackPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BackPath" + cValue, "", wsIniPath);
            }

            string ckmsg = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ckTcMsg", "", wsIniPath);
            bool cg = false;
            bool.TryParse(ckmsg, out cg);
            ckTcMsg.Checked = cg;
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
            IniFileHelper.WriteIniData("workStationNum", WorkStation + "IP", this.tbIP.Text, wsIniPath);

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
            cbType.Enabled = !cbType.Enabled;
            tbReadRate.ReadOnly = !tbReadRate.ReadOnly;
            tbIP.ReadOnly = !tbIP.ReadOnly;
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
            switch (cbType.SelectedValue.ToString())
            {
                case "1":  //伍洲AOI
                    isTrue = readWZ();
                    break;
                case "2": //神州AOI
                    isTrue = readSZ(); //readWZ();
                    break;
                case "3": //YAMAHAAOI
                    isTrue = readYMH();
                    break;
                case "4": //快克AOI
                    isTrue = readKK();
                    break;
                default:
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    lblState.Text = "没有符合解析的设备类型...";
                    lblStateTitle.ForeColor = Color.Red;
                    lblState.ForeColor = Color.Red;
                    SetEnable();
                    isTrue = false;
                    break;
            }
            if (!isTrue)
                btnEnd_Click(null,null);
            timer2.Enabled = isTrue;
        }
        #endregion

        #region 伍洲AOI  txt文本
        private bool readWZ()
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
                if (lines.Length < 12)
                    continue;
                #endregion

                //文本未被占用,读取文本,
                string snCode = lines[1].Trim();
                string spec = lines[0].Trim();  //项目名称
                string startTime = lines[6].Trim();
                string endTime = lines[7].Trim();
                string result = lines[8].Trim();
                string faceCode = lines[9].Trim();//面别
                string num = lines[10].Trim(); //元件数
                ecCode = "";
                string ngNum = "";
                string errDw = "";
                if (!"PASS".Equals(result.ToUpper()) && !"RPASS".Equals(result.ToUpper()))
                {
                    ecCode = "|AOI001";
                    ngNum = "1";
                }

                #region 获取明细
                DataTable dt = new DataTable("mx");
                dt.Columns.Add(new DataColumn("WIP_AOI_POINT", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_AOI_SPEC", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_AOI_RES", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_AOI_PURL", typeof(string)));

                DataRow dr = null;
                for (int i = 12; i < lines.Length; i++)
                {
                    string sa = lines[i].ToString();
                    if (!sa.Contains(","))
                        continue;
                    string[] ssa = sa.Split(',');
                    dr = dt.NewRow();
                    dr["WIP_AOI_POINT"] = ssa[0];
                    dr["WIP_AOI_SPEC"] = ssa[1];
                    dr["WIP_AOI_RES"] = ssa[2];

                    if (!"".Equals(ecCode) && !"OK".Equals(ssa[2]))
                        errDw +=  "," + ssa[0];

                    if (ssa.Length >= 5)
                    {
                        //string[] path = ssa[4].Split('\\');
                        //string lastFile = path[path.Length - 2] + @"\" + path[path.Length - 1];
                        //dr["WIP_AOI_PURL"] = "\\" + tbIP.Text + @"\" + lastFile;
                        //伍洲直接取值保存，不需要拼接 
                        dr["WIP_AOI_PURL"] = ssa[4];
                    }
                    else
                    {
                        dr["WIP_AOI_PURL"] = "";
                    }

                    dt.Rows.Add(dr);
                }
                if (errDw.Length > 0)
                    errDw = "|" + errDw.TrimStart(',');
                #endregion

                ht = run(snCode, ecCode, ngNum, errDw, "");
                string res = ht["RES"].ToString();
                string wis = res.Substring(0, 2);
                ht.Clear();

                if (ckTcMsg.Checked && "NG".Equals(wis))
                {
                    CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                }

                #region 数据保存
                string guid = Guid.NewGuid().ToString("N");
                string sqlAOI = string.Format("INSERT INTO T_WIP_AOI_INSPECT(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, WIP_AOI_SN, WIP_AOI_SPEC, WIP_AOI_RES," +
                    "WIP_AOI_START, WIP_AOI_END, WIP_AOI_LINE, WIP_AOI_FACE, WIP_AOI_QYT, WIP_AOI_TYPE, WIP_MES_RES, WIP_MES_MSG)VALUES ('{0}','{1}',SYSDATE," +
                    "'{2}','{3}','{4}','{5}',to_date('{6}','yyyy-mm-dd hh24:mi:ss'),to_date('{7}','yyyy-mm-dd hh24:mi:ss'),'{8}','{9}','{10}','{11}','{12}','{13}')"
                    , guid, m_UsrCode, data_auth, snCode, spec, result, startTime, endTime, m_ProductLine, faceCode, num, cbType.SelectedValue.ToString(), wis, res);
                try
                {
                    dbHelper.ExecuteSql(sqlAOI);
                }
                catch (Exception err)
                {
                    SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sqlAOI);
                }

                StringBuilder sb = new StringBuilder("BEGIN ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("INSERT INTO T_WIP_AOI_INSPECT_DETAIL(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ");
                    sb.Append("WIP_ITEM_ID,WIP_AOI_POINT, WIP_AOI_SPEC, WIP_AOI_RES, WIP_AOI_PURL)VALUES ");
                    sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                    sb.Append(guid + "','" + dt.Rows[i]["WIP_AOI_POINT"] + "','" + dt.Rows[i]["WIP_AOI_SPEC"] + "','");
                    sb.Append(dt.Rows[i]["WIP_AOI_RES"] + "','" + dt.Rows[i]["WIP_AOI_PURL"] + "'); ");
                }
                sb.Append("END;");
                try
                {
                    if (sb.Length >20)
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
                isBreak++;
            }
            DBHelper.Instance.FlashDBObject(DBCode);
            return true;
        }
        #endregion

        #region 神州AOI
        private bool readSZ()
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
                #endregion

                //解析文件名称
                string fileName = Path.GetFileName(file);

                try
                {
                    string[] fns = fileName.Split('_');
                    string snCode = fns[0].Trim();
                    string snType = fns[1].Trim();  //SN条码状态
                    string spec = "";
                    try
                    {
                        spec = fns[2].Trim();
                    }
                    catch (Exception) { }

                    ecCode = "";
                    string ngNum = "";
                    string errDw = "";
                    if (!"PASS".Equals(snType.ToUpper()))
                    {
                        ecCode = "|AOI001";
                        ngNum = "1";
                    }
                    #region 获取明细
                    DataTable dt = new DataTable("mx");
                    dt.Columns.Add(new DataColumn("WIP_AOI_POINT", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_SPEC", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_RES", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_PURL", typeof(string)));
                    //PASS不保存明细
                    if (!"PASS".Equals(snType.ToUpper()))
                    {
                        DataRow dr = null;
                        for (int i = 5; i < lines.Length; i++)
                        {
                            string sa = lines[i].ToString();
                            if (!"".Equals(sa.Trim()))
                            {
                                string[] ssa = sa.Split(';');
                                dr = dt.NewRow();

                                dr["WIP_AOI_POINT"] = ssa[1];//点位
                                dr["WIP_AOI_SPEC"] = ssa[0]; //规格
                                dr["WIP_AOI_RES"] = "NG";  //结果

                                if (!"".Equals(ecCode))
                                    errDw += "," + ssa[2];

                                if (ssa.Length >= 5)
                                {
                                    string[] path = ssa[4].Split('\\');
                                    if (path.Length >= 2)
                                    {
                                        string lastFile = path[path.Length - 4] + @"\" + path[path.Length - 3] + @"\" + path[path.Length - 2] + @"\" + path[path.Length - 1];
                                        dr["WIP_AOI_PURL"] = @"\\" + tbIP.Text + @"\" + lastFile;
                                    }
                                    else
                                    {
                                        dr["WIP_AOI_PURL"] = ssa[4];
                                    }
                                }
                                else
                                {
                                    dr["WIP_AOI_PURL"] = "";
                                }
                                dt.Rows.Add(dr);
                            }

                        }
                    }
                    if (errDw.Length > 0)
                        errDw = "|" + errDw.TrimStart(',');
                    #endregion

                    //解析状态不为PASS/OK/NG/FAIL的过站NG
                    string wis = "";
                    string res = "";
                    if ("PASS".Equals(snType.ToUpper()) || "OK".Equals(snType.ToUpper()) || "NG".Equals(snType.ToUpper()) || "FAIL".Equals(snType.ToUpper()))
                    {
                        ht = run(snCode, ecCode, ngNum, errDw, "");
                        res = ht["RES"].ToString();
                        wis = res.Substring(0, 2);
                        ht.Clear();
                    }
                    else
                    {
                        wis = "NG";
                        res = "解析产品状态异常";
                    }

                    if (ckTcMsg.Checked && "NG".Equals(wis))
                    {
                        CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                    }

                    if ("PASS".Equals(snType.ToUpper()) || "OK".Equals(snType.ToUpper()) || "NG".Equals(snType.ToUpper()) || "FAIL".Equals(snType.ToUpper()))
                    {
                        #region 数据保存
                        string guid = Guid.NewGuid().ToString("N");
                        string sqlAOI = string.Format("INSERT INTO T_WIP_AOI_INSPECT(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, WIP_AOI_SN, WIP_AOI_SPEC, WIP_AOI_RES," +
                            "WIP_AOI_LINE, WIP_AOI_FACE, WIP_AOI_QYT, WIP_AOI_TYPE, WIP_MES_RES, WIP_MES_MSG,WIP_AOI_START)VALUES ('{0}','{1}',SYSDATE," +
                            "'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',SYSDATE)", guid, m_UsrCode, data_auth,
                            snCode, spec, snType, m_ProductLine, "", "", cbType.SelectedValue.ToString(), wis, res);
                        try
                        {
                            dbHelper.ExecuteSql(sqlAOI);
                        }
                        catch (Exception err)
                        {
                            SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sqlAOI);
                        }

                        StringBuilder sb = new StringBuilder("BEGIN ");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sb.Append("INSERT INTO T_WIP_AOI_INSPECT_DETAIL(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ");
                            sb.Append("WIP_ITEM_ID,WIP_AOI_POINT, WIP_AOI_SPEC, WIP_AOI_RES, WIP_AOI_PURL)VALUES ");
                            sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                            sb.Append(guid + "','" + dt.Rows[i]["WIP_AOI_POINT"] + "','" + dt.Rows[i]["WIP_AOI_SPEC"] + "','");
                            sb.Append(dt.Rows[i]["WIP_AOI_RES"] + "','" + dt.Rows[i]["WIP_AOI_PURL"] + "'); ");
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
                    }

                    #region 文件名称变更
                    string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                    File.Move(file, newPath);
                    #endregion

                    dgvFill(snCode, wis, res);

                }
                catch (Exception ex1)
                {
                    #region 文件名称变更
                    string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), "Error" + "_" + DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                    File.Move(file, newPath);
                    #endregion
                    dgvFill(fileName, "NG", ex1.ToString());
                }
                isBreak++;
            }
            return true;
        }
        #endregion

        #region YAMAHAAOI
        private bool readYMH()
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
                #endregion

                //解析文件名称
                string fileName = Path.GetFileName(file);

                try
                {
                    string[] fns = fileName.Split('_');
                    string snCode = fns[0].Trim();
                    string snType = fns[1].Trim();  //SN条码状态
                    string spec = "";
                    try
                    {
                        spec = fns[2].Trim();
                    }
                    catch (Exception) { }

                    ecCode = "";
                    string ngNum = "";
                    string errDw = "";
                    if (!"PASS".Equals(snType.ToUpper()) && !"OK".Equals(snType.ToUpper()))
                    {
                        ecCode = "|AOI001";
                        ngNum = "1";
                    }
                    #region 获取明细
                    DataTable dt = new DataTable("mx");
                    dt.Columns.Add(new DataColumn("WIP_AOI_POINT", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_SPEC", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_RES", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_PURL", typeof(string)));
                    //PASS不保存明细
                    if (!"PASS".Equals(snType.ToUpper()))
                    {
                        DataRow dr = null;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string sa = lines[i].ToString();
                            string[] ssa = sa.Split(',');
                            dr = dt.NewRow();

                            dr["WIP_AOI_POINT"] = ssa[9];//点位
                            dr["WIP_AOI_SPEC"] = ssa[8]; //规格
                            dr["WIP_AOI_RES"] = ssa[7];  //结果

                            if (!"".Equals(ecCode) && !"OK".Equals(ssa[7]))
                                errDw += "," + ssa[9];

                            if (ssa.Length >= 17)
                            {
                                string[] path = ssa[16].Split('\\');
                                if (path.Length >= 2)
                                {
                                    string lastFile = path[path.Length - 2] + @"\" + path[path.Length - 1];
                                    dr["WIP_AOI_PURL"] = "\\" + tbIP.Text + @"\" + lastFile;
                                }
                                else
                                {
                                    dr["WIP_AOI_PURL"] = ssa[16];
                                }
                            }
                            else
                            {
                                dr["WIP_AOI_PURL"] = "";
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (errDw.Length > 0)
                        errDw = "|" + errDw.TrimStart(',');
                    #endregion

                    //解析状态不为PASS/OK/NG/FAIL的过站NG
                    string wis = "";
                    string res = "";
                    if ("PASS".Equals(snType.ToUpper()) || "OK".Equals(snType.ToUpper()) || "NG".Equals(snType.ToUpper()) || "FAIL".Equals(snType.ToUpper()))
                    {
                        ht = run(snCode, ecCode, ngNum, errDw, "");
                        res = ht["RES"].ToString();
                        wis = res.Substring(0, 2);
                        ht.Clear();
                    }
                    else
                    {
                        wis = "NG";
                        res = "解析产品状态异常";
                    }


                    if (ckTcMsg.Checked && "NG".Equals(wis))
                    {
                        CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                    }

                    if ("PASS".Equals(snType.ToUpper()) || "OK".Equals(snType.ToUpper()) || "NG".Equals(snType.ToUpper()) || "FAIL".Equals(snType.ToUpper()))
                    {
                        #region 数据保存
                        string guid = Guid.NewGuid().ToString("N");
                        string sqlAOI = string.Format("INSERT INTO T_WIP_AOI_INSPECT(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, WIP_AOI_SN, WIP_AOI_SPEC, WIP_AOI_RES," +
                            "WIP_AOI_LINE, WIP_AOI_FACE, WIP_AOI_QYT, WIP_AOI_TYPE, WIP_MES_RES, WIP_MES_MSG,WIP_AOI_START)VALUES ('{0}','{1}',SYSDATE," +
                            "'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',SYSDATE)", guid, m_UsrCode, data_auth,
                            snCode, spec, snType, m_ProductLine, "", "", cbType.SelectedValue.ToString(), wis, res);
                        try
                        {
                            dbHelper.ExecuteSql(sqlAOI);
                        }
                        catch (Exception err)
                        {
                            SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sqlAOI);
                        }

                        StringBuilder sb = new StringBuilder("BEGIN ");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sb.Append("INSERT INTO T_WIP_AOI_INSPECT_DETAIL(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ");
                            sb.Append("WIP_ITEM_ID,WIP_AOI_POINT, WIP_AOI_SPEC, WIP_AOI_RES, WIP_AOI_PURL)VALUES ");
                            sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                            sb.Append(guid + "','" + dt.Rows[i]["WIP_AOI_POINT"] + "','" + dt.Rows[i]["WIP_AOI_SPEC"] + "','");
                            sb.Append(dt.Rows[i]["WIP_AOI_RES"] + "','" + dt.Rows[i]["WIP_AOI_PURL"] + "'); ");
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
                    }

                    #region 文件名称变更
                    string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                    File.Move(file, newPath);
                    #endregion

                    dgvFill(snCode, wis, res);

                }
                catch (Exception ex1)
                {
                    #region 文件名称变更
                    string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), "Error" + "_" + DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                    File.Move(file, newPath);
                    #endregion
                    dgvFill(fileName, "NG", ex1.ToString());
                }
                isBreak++;
            }
            DBHelper.Instance.FlashDBObject(DBCode);
            return true;
        }
        #endregion

        #region 快克AOI
        private bool readKK()
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
                #endregion

                //解析文件名称
                string fileName = Path.GetFileName(file);

                try
                {
                    string[] fns = fileName.Split('_');
                    string snCode = fns[0].Trim();
                    string snType = fns[1].Trim();  //SN条码状态
                    string spec = "";
                    try
                    {
                        spec = fns[2].Trim();
                    }
                    catch (Exception) { }

                    ecCode = "";
                    string ngNum = "";
                    string errDw = "";
                    if (!"PASS".Equals(snType.ToUpper()) && !"OK".Equals(snType.ToUpper()))
                    {
                        ecCode = "|AOI001";
                        ngNum = "1";
                    }
                    #region 获取明细
                    DataTable dt = new DataTable("mx");
                    dt.Columns.Add(new DataColumn("WIP_AOI_POINT", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_SPEC", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_RES", typeof(string)));
                    dt.Columns.Add(new DataColumn("WIP_AOI_PURL", typeof(string)));
                    //PASS不保存明细
                    if (!"PASS".Equals(snType.ToUpper()))
                    {
                        DataRow dr = null;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string sa = lines[i].ToString();
                            string[] ssa = sa.Split(',');
                            dr = dt.NewRow();

                            dr["WIP_AOI_POINT"] = ssa[1];//点位
                            dr["WIP_AOI_SPEC"] = ssa[0]; //规格
                            dr["WIP_AOI_RES"] = ssa[2];  //结果

                            if (!"".Equals(ecCode) && !"OK".Equals(ssa[2]))
                                errDw += "," + ssa[1];

                            if (ssa.Length >= 4)
                            {
                                string[] path = ssa[3].Split('\\');
                                if (path.Length >= 2)
                                {
                                    string lastFile = path[path.Length - 2] + @"\" + path[path.Length - 1];
                                    dr["WIP_AOI_PURL"] = "\\" + tbIP.Text + @"\" + lastFile;
                                }
                                else
                                {
                                    dr["WIP_AOI_PURL"] = ssa[3];
                                }
                            }
                            else
                            {
                                dr["WIP_AOI_PURL"] = "";
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (errDw.Length > 0)
                        errDw = "|" + errDw.TrimStart(',');
                    #endregion

                    //解析状态不为PASS/OK/NG/FAIL的过站NG
                    string wis = "";
                    string res = "";
                    if ("PASS".Equals(snType.ToUpper()) || "OK".Equals(snType.ToUpper()) || "NG".Equals(snType.ToUpper()) || "FAIL".Equals(snType.ToUpper()))
                    {
                        ht = run(snCode, ecCode, ngNum, errDw, "");
                        res = ht["RES"].ToString();
                        wis = res.Substring(0, 2);
                        ht.Clear();
                    }
                    else
                    {
                        wis = "NG";
                        res = "解析产品状态异常";
                    }


                    if (ckTcMsg.Checked && "NG".Equals(wis))
                    {
                        CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                    }

                    if ("PASS".Equals(snType.ToUpper()) || "OK".Equals(snType.ToUpper()) || "NG".Equals(snType.ToUpper()) || "FAIL".Equals(snType.ToUpper()))
                    {
                        #region 数据保存
                        string guid = Guid.NewGuid().ToString("N");
                        string sqlAOI = string.Format("INSERT INTO T_WIP_AOI_INSPECT(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, WIP_AOI_SN, WIP_AOI_SPEC, WIP_AOI_RES," +
                            "WIP_AOI_LINE, WIP_AOI_FACE, WIP_AOI_QYT, WIP_AOI_TYPE, WIP_MES_RES, WIP_MES_MSG,WIP_AOI_START)VALUES ('{0}','{1}',SYSDATE," +
                            "'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',SYSDATE)", guid, m_UsrCode, data_auth,
                            snCode, spec, snType, m_ProductLine, "", "", cbType.SelectedValue.ToString(), wis, res);
                        try
                        {
                            dbHelper.ExecuteSql(sqlAOI);
                        }
                        catch (Exception err)
                        {
                            SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sqlAOI);
                        }

                        StringBuilder sb = new StringBuilder("BEGIN ");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sb.Append("INSERT INTO T_WIP_AOI_INSPECT_DETAIL(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ");
                            sb.Append("WIP_ITEM_ID,WIP_AOI_POINT, WIP_AOI_SPEC, WIP_AOI_RES, WIP_AOI_PURL)VALUES ");
                            sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                            sb.Append(guid + "','" + dt.Rows[i]["WIP_AOI_POINT"] + "','" + dt.Rows[i]["WIP_AOI_SPEC"] + "','");
                            sb.Append(dt.Rows[i]["WIP_AOI_RES"] + "','" + dt.Rows[i]["WIP_AOI_PURL"] + "'); ");
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
                    }

                    #region 文件名称变更
                    string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                    File.Move(file, newPath);
                    #endregion

                    dgvFill(snCode, wis, res);

                }
                catch (Exception ex1)
                {
                    #region 文件名称变更
                    string newPath = Path.Combine(CreatFolderByDate(tbBackPath.Text), "Error" + "_" + DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                    File.Move(file, newPath);
                    #endregion
                    dgvFill(fileName, "NG", ex1.ToString());
                }
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
        /// <param name="ecCode">不良点位</param>
        /// <param name="csCount">不良点数</param>
        /// <param name="ht"></param>
        /// <returns></returns>
        private Hashtable run(string SN, string ecCode, string ngNum, string psCode,string csCount)
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
                //20220607  P_C_SAVE_SN_TEST存储修改为只能过站镭雕记录的SN，导致AOI无法正常使用，改用xxx2存储
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
                string tName = "[" + ip + "]:AOI " + SN;
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
