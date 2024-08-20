using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseModel;
using System.IO;
using BaseModel.Logs;
using System.Collections;
using WorkStation.FunClass;

/**
 * 菱电SPI
 *   SPI csv格式
 *   20220606 SPI设备升级，全部统一最新格式，删除旧明细不要的列，添加新字段
 */ 
namespace WorkStation
{
    public partial class TimerReadSPI : CUserControl, ICommonWorkStationCtl
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

        public TimerReadSPI()
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
            tbReadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath", "", wsIniPath);
            tbBackPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BackPath", "", wsIniPath);
            tbReadRate.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Fre", "", wsIniPath);
            tbIP.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "IP", "", wsIniPath);

            ip = Computer.Instance().IpAddress;
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
            isTrue = MachineDataRead();
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
                if (lines.Length < 7)
                    continue;
                #endregion

                //文本未被占用,读取文本,
                string snCode = Path.GetFileNameWithoutExtension(file);
                string ModelName = lines[1].Split(',')[0].Trim();
                string LineNumber = lines[1].Split(',')[1].Trim();
                string BoardStatus = lines[3].Split(',')[1].Trim();
                ecCode = "";
                string ngNum = "";
                if ("NG".Equals(BoardStatus.ToUpper()))
                {
                    ecCode = "|SPI001";
                    ngNum = "1";
                }

                ht = run(snCode, ecCode, ngNum);
                string res = ht["RES"].ToString();
                string wis = res.Substring(0, 2);
                ht.Clear();
                if (ckTcMsg.Checked && "NG".Equals(wis))
                {
                    CWorkFlowControlHelper.MsgMessageBox("条码：" + snCode + " 过站失败！", "Error");
                }

                #region 获取明细中含有非Good的信息
                DataTable dt = new DataTable("mx");
                dt.Columns.Add(new DataColumn("WIP_PADID", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_AREA1", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_HEIGHT2", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_VOLUME1", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_XOFFSET", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_YOFFSET", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_PADSIZEX", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_PADSIZEY", typeof(string)));

                dt.Columns.Add(new DataColumn("WIP_AREA2", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_HEIGHT1", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_VOLUME2", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_XOFFSET2", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_YOFFSET2", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_RESULT", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_ERRCODE", typeof(string)));
                dt.Columns.Add(new DataColumn("WIP_URL", typeof(string)));

                DataRow dr = null;
                for (int i = 6; i < lines.Length; i++)
                {
                    string sa = lines[i].ToString();
                    string[] ssa = sa.Split(',');

                    string st = ssa[19].ToString().Trim().ToUpper();
                    if ("PASS".Equals(st) || "FAIL".Equals(st))
                    {
                        if (!"FAIL".Equals(ssa[19]))
                        {
                            continue;//不是fail的明细跳过
                        }
                        dr = dt.NewRow();
                        dr["WIP_PADID"] = ssa[0];
                        dr["WIP_AREA1"] = ssa[7];
                        dr["WIP_AREA2"] = ssa[8];

                        dr["WIP_VOLUME1"] = ssa[9];
                        dr["WIP_VOLUME2"] = ssa[10];

                        dr["WIP_HEIGHT1"] = ssa[11];
                        dr["WIP_HEIGHT2"] = ssa[12];

                        dr["WIP_XOFFSET"] = ssa[13];
                        dr["WIP_XOFFSET2"] = ssa[14];

                        dr["WIP_YOFFSET"] = ssa[15];
                        dr["WIP_YOFFSET2"] = ssa[16];

                        dr["WIP_PADSIZEX"] = ssa[17];
                        dr["WIP_PADSIZEY"] = ssa[18];

                        dr["WIP_RESULT"] = ssa[19];
                        dr["WIP_ERRCODE"] = ssa[20];
                        if (ssa.Length >= 21)
                        {
                            string[] path = ssa[21].Split('\\');
                            if (path.Length >= 2)
                            {
                                string lastFile = path[path.Length - 2] + @"\" + path[path.Length - 1];
                                dr["WIP_URL"] = @"\\" + tbIP.Text + @"\" + lastFile;
                            }
                            else
                                dr["WIP_URL"] = "";
                        }
                        else
                            dr["WIP_URL"] = "";

                        dt.Rows.Add(dr);
                    }
                }
                #endregion

                #region 数据保存
                string guid = Guid.NewGuid().ToString("N");
                string sqlSPI = string.Format("INSERT INTO T_WIP_SPI_ITEM(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, WIP_ITEM_SN, WIP_ITEM_SPEC,WIP_ITEM_LINE," +
                    "WIP_ITEM_RESULT,WIP_ITEM_STATUS,WIP_ITEM_MSG)VALUES('{0}','{1}',SYSDATE,'{2}','{3}','{4}','{5}','{6}','{7}','{8}')"
                    , guid, m_UsrCode, data_auth, snCode, ModelName, m_ProductLine, BoardStatus, wis, res);
                try
                {
                    dbHelper.ExecuteSql(sqlSPI);
                }
                catch (Exception err)
                {
                    SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sqlSPI); 
                }

                StringBuilder sb = new StringBuilder("BEGIN ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("INSERT INTO T_WIP_SPI_DETAIL(ID,CREATE_USER,CREATE_TIME,DATA_AUTH,WIP_PADID,WIP_URL,");
                    sb.Append("WIP_AREA1,WIP_HEIGHT2,WIP_VOLUME1,WIP_XOFFSET,WIP_YOFFSET,WIP_PADSIZEX,WIP_PADSIZEY,");
                    sb.Append("WIP_AREA2,WIP_HEIGHT1,WIP_VOLUME2,WIP_RESULT,WIP_ERRCODE,WIP_ITEM_ID,WIP_XOFFSET2,WIP_YOFFSET2)VALUES ");
                    sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                    sb.Append(dt.Rows[i]["WIP_PADID"] + "','" + dt.Rows[i]["WIP_URL"] + "','" + dt.Rows[i]["WIP_AREA1"] + "','");
                    sb.Append(dt.Rows[i]["WIP_HEIGHT2"] + "','" + dt.Rows[i]["WIP_VOLUME1"] + "','" + dt.Rows[i]["WIP_XOFFSET"] + "','");
                    sb.Append(dt.Rows[i]["WIP_YOFFSET"] + "','" + dt.Rows[i]["WIP_PADSIZEX"] + "','" + dt.Rows[i]["WIP_PADSIZEY"] + "','");
                    sb.Append(dt.Rows[i]["WIP_AREA2"] + "','" + dt.Rows[i]["WIP_HEIGHT1"] + "','" + dt.Rows[i]["WIP_VOLUME2"] + "','");
                    sb.Append(dt.Rows[i]["WIP_RESULT"] + "','" + dt.Rows[i]["WIP_ERRCODE"] + "','" + guid + "','");
                    sb.Append(dt.Rows[i]["WIP_XOFFSET2"] + "','" + dt.Rows[i]["WIP_YOFFSET2"] + "');");
                }
                sb.Append("END;");
                try
                {
                    if (sb.Length > 20)
                    {
                        dbHelper.ExecuteSql(sb.ToString()); 
                    }
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

                dgvFill(snCode, BoardStatus, wis, res);
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
                ht = dbHelper.htExecuteNonQuery(ListDBParameters, "P_C_SAVE_SN_TEST");
            }
            catch (Exception err)
            {
                htDmp.Add("RES", "NG:存储执行异常！");
                ht.Add("RES","NG:存储执行异常！");
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
        /// <param name="bStatus">spi文件条码结果</param>
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
