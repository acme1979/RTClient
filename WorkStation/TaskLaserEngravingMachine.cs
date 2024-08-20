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
using System.Threading;
using System.IO;
using BaseModel.Logs;

/**
 * 菱电镭雕机对接  
 * 解析文件把数据存放到PCB板关联信息表中
 * 22-01-17修改 数据不在存PCB关联表，新建一个镭雕信息表进行记录
 * 22-01-19修改 一个SN一行，按行记录信息
 */ 
namespace WorkStation
{
    public partial class TaskLaserEngravingMachine : CUserControl, ICommonWorkStationCtl
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
        public TaskLaserEngravingMachine()
        {
            InitializeComponent();
            ListBox.CheckForIllegalCrossThreadCalls = false;
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
                if (lines.Length <= 0)
                    continue;
                #endregion

                //文本未被占用,读取文本,
                string msg = "";
                StringBuilder sb = new StringBuilder();
                string groupId = Guid.NewGuid().ToString("D"); // 9af7f46a-ea52-4aa3-b8c3-9fd484c2af12
                
                sb.Append("BEGIN ");
                foreach (string line in lines)
                {
                    sb.Append("INSERT INTO T_WIP_LASERENGRAVING (ID,DATA_AUTH,CREATE_USER,CREATE_TIME,SN,GROUP_ID) VALUES ('");
                    sb.Append(Guid.NewGuid().ToString("N") + "','" + data_auth + "','" + m_UsrCode + "',SYSDATE,'" + line + "','" + groupId + "');");
                }
                sb.Append("END;");

                /*sb.Append("BEGIN ");
                sb.Append("INSERT INTO T_WIP_PCB_ITEM (ID,WPI_ID, SPLIT_GROUP_ID,WPI_EMP, WPI_TIME,DATA_AUTH) VALUES ('");
                sb.Append(Guid.NewGuid().ToString() + "','" + Guid.NewGuid().ToString() + "','" + guid + "','");
                sb.Append(m_UsrCode + "',SYSDATE,'" + data_auth + "');");
                for (int i = 0; i < 2; i++)
                {

                    sb.Append("INSERT INTO T_CO_SN_RELATION (ID, SPLIT_FLAG, SPLIT_TIME, SPLIT_EMP, ");
                    sb.Append("SPLIT_GROUP_ID,SPLIT_GROUP_OLDID, DATA_AUTH,NEW_SN, OLD_SN)");
                    sb.Append("VALUES ('" + Guid.NewGuid().ToString() + "','N',SYSDATE,'" + m_UsrCode + "','");
                    sb.Append(guid + "','" + guid + "','" + data_auth + "','");
                    if (i == 0)
                    {
                        sb.Append(snCode1 + "','" + snCode1 + "');");
                    }
                    else
                    {
                        sb.Append(snCode2 + "','" + snCode2 + "');");
                    }
                }
                sb.Append("END;");*/

                try
                {
                    if (sb.Length > 20)
                        dbHelper.ExecuteSql(sb.ToString());
                }
                catch (Exception err)
                {
                    SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sb.ToString());
                    if (err.ToString().Contains("违反唯一约束条件"))
                        msg = Path.GetFileName(file) + "     保存失败！文件内SN存在生产记录不能再次生产";
                    else
                        msg = Path.GetFileName(file) + "     保存失败！" + err.ToString();
                    ShowListBoxMsg(msg);
                    return true;
                }
                #region 文件名称变更
                string newPath = Path.Combine(tbBackPath.Text, DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
                File.Move(file, newPath);
                #endregion

                msg = Path.GetFileName(file) + "     保存成功！";
                ShowListBoxMsg(msg);
                isBreak++;
            }
            DBHelper.Instance.FlashDBObject(DBCode);
            return true;
        }
        #endregion

        public void ShowListBoxMsg(string msg)
        {
            Thread th = new Thread(new ThreadStart(() =>
            {
                listBoxMsg.Items.Insert(0, msg);
            }));
            th.Start();
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
    }
}
