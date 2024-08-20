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
using System.IO;
using ExcelReport;
using System.Data.OleDb;


namespace WorkStation
{
    public partial class TimerReadECU : CUserControl, ICommonWorkStationCtl
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

        public TimerReadECU()
        {
            InitializeComponent();
            tbReadPath.Click +=new EventHandler(tbReadPath_Click);
            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);

            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);
        }

        #region InitData
        public void InitData()
        {
            tbReadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath", "", wsIniPath);
            tbReadRate.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Fre", "", wsIniPath);

            tbFileName.Text = GetFileName();
        }
        #endregion

        #region 读取文件设置
        private void tbReadPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个文件";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbReadPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ReadPath", this.tbReadPath.Text, wsIniPath);
            }
        } 

        //获取当前读取文件名
        private string GetFileName()
        {
            string fileName = DateTime.Now.ToLongDateString().ToString() + ".xls";
            return fileName;
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
            string fileName = tbReadPath.Text + @"\" + tbFileName.Text;
            if (!File.Exists(fileName))
            {
                MessageBox.Show("读取文件不存在");
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
        }
        #endregion

        #region SetEnable()
        private void SetEnable()
        {
            tbReadPath.Enabled = !tbReadPath.Enabled;
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

            //判断当前时间是否在00:00:30-00:01:00之间，在的话，更新窗体文件名
            //获取当前系统时间并判断是否为服务时间
            TimeSpan nowDt = DateTime.Now.TimeOfDay;

            TimeSpan workStartDT = DateTime.Parse("00:00:30").TimeOfDay;
            TimeSpan workEndDT = DateTime.Parse("00:02:30").TimeOfDay;
            if (nowDt > workStartDT && nowDt < workEndDT)
            {
                tbFileName.Text = GetFileName();
            }
        }
        #endregion

        #region timer2_Tick
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            timer2.Enabled = MachineDataRead();
        }
        #endregion


        #region  业务处理 MachineDataRead()
        private bool MachineDataRead()
        {
            string fileName = tbReadPath.Text + @"\" + tbFileName.Text;
            if (!File.Exists(fileName)) 
            {
                dgvFill("", "", "读取文件不存在");
                return true;
            }

            string info = File.ReadAllText(fileName, System.Text.Encoding.Default);


            //ArrayList result = ExcelRead.GetExcelSheetsNames(fileName);

           // DataSet ds = LoadDataFromExcel(fileName, "2021年12月20日");

            //ArrayList result = GetExcelSheetsNames(fileName);



            //dbHelper = DBHelper.Instance.GetDBInstace(DBCode);
            //dgvFill(PcbSN, SN, msg);
            return true;
        }
        #endregion

        #region 读取Excel并返回数据表
        public static DataSet LoadDataFromExcel(string filePath, string sheetName)
        {
            System.Data.OleDb.OleDbConnection oleConn = null;
            System.Data.OleDb.OleDbDataAdapter oleDa = null;
            try
            {
                DataSet ds = new DataSet();
                string strConn;
                if (Path.GetExtension(filePath).ToUpper() == ".XLS")
                {
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";
                }
                else
                {
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
                }
                if (strConn == "")
                {
                    throw new Exception("CS_EXCEL_NOT_INSTALL");
                }
                oleConn = new System.Data.OleDb.OleDbConnection(strConn);
                string selectStr = "select * from [" + sheetName + "]";
                oleDa = new System.Data.OleDb.OleDbDataAdapter(selectStr, oleConn);
                oleDa.Fill(ds, sheetName);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //去掉列标题中的空格
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        col.ColumnName = col.ColumnName.Replace(" ", "");
                    }
                    //去掉空行
                    for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        //判断是否空行
                        bool exist = false;
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            if (dr[col].ToString() != "")
                            {
                                exist = true;
                                break;
                            }
                        }
                        //如果是空行，则移除当前行
                        if (!exist)
                        {
                            ds.Tables[0].Rows.RemoveAt(i);
                        }
                    }
                }
                return ds;
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                //Excel版本不兼容，作出提示；另外，当表单的第一个表的表名不是'Sheet1'时，也会catch该异常
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                //CMessageBox.ShowError("Import Excel file failed", e);
                throw new Exception("Import Excel file failed", ex);
            }
            finally
            {
                if (oleConn != null)
                {
                    oleConn.Close();
                    oleConn.Dispose();
                }
                if (oleDa != null)
                {
                    oleDa.Dispose();
                }
                GC.Collect();
            }
        }
        #endregion


        #region GetExcelSheetsNames
        /// <summary>
        /// 获得Excel文件所有sheet的名称
        /// </summary>
        /// <param name="Conn">Excel的OleDb连接</param>
        /// <returns>sheet名称数组</returns>
        public static ArrayList GetExcelSheetsNames(string filePath)
        {
            string strConn;
            if (Path.GetExtension(filePath).ToUpper() == ".XLS")
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";
            }
            else
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
            }


            strConn = string.Format("Provider=Microsoft.Jet.OLEDB.{0}.0;" +
                "Extended Properties=\"Excel {1}.0;HDR={2};IMEX=1;\";" +
                "data source={3};",
                (".xls1" == ".xls" ? 4 : 12), (".xls1" == ".xls" ? 8 : 12), (true ? "Yes" : "NO"), filePath);
            string strCom = " SELECT * FROM [Sheet1$]";

            OleDbConnection OleConn = new OleDbConnection(strConn);

            DataSet ds = new DataSet();
            using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, OleConn))
            {
                OleConn.Open();
                myCommand.Fill(ds);
            }

            ArrayList result;
            try
            {
                OleConn.Open();
                result = new ArrayList();
                System.Data.DataTable sheetNames = OleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow dr in sheetNames.Rows)
                {
                    result.Add(dr[2]);
                }
            }
            finally
            {
                if (OleConn != null)
                {
                    OleConn.Close();
                    OleConn.Dispose();
                }
                if (OleConn != null)
                {
                    OleConn.Dispose();
                }
                GC.Collect();
            }
            return result;
        }
        #endregion





        #region dgvFill
        /// <summary>
        /// dataGridView表格填充
        /// </summary>
        /// <param name="PcbSN">PcbSN</param>
        /// <param name="PcbSN">总成条码</param>
        /// <param name="msg">msg</param>
        private void dgvFill(string PcbSN, string SN,string msg)
        {
            if (dataGridView1.Rows.Count >= 80)
                dataGridView1.Rows.Clear();

            int index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells["PcbSN"].Value = PcbSN;
            dataGridView1.Rows[index].Cells["SN"].Value = SN;
            dataGridView1.Rows[index].Cells["Time"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
