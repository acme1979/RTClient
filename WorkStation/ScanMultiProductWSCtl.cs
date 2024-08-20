using System.Collections;
using System.Collections.Generic;
using System.Data;
using BaseModel;
using System;
using System.Windows.Forms;

namespace WorkStation
{
    public partial class ScanMultiProductWSCtl : CUserControl, ICommonWorkStationCtl
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

        //产品条码内容
        private string m_BarString = "";
        /// <summary>
        /// 用于存放组合产品的工艺信息
        /// </summary>
        private List<AssembleProductInfo> listAssembleProduct = new List<AssembleProductInfo>();

        #endregion

        DBMySqlHelper MySqlHelper = null;
        public ScanMultiProductWSCtl()
        {
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click);
        }

        void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string server = textBox1.Text;
                string dbName = textBox2.Text;
                string userId = textBox3.Text;
                string passWord = textBox4.Text;
                string kou = textBox5.Text;
                MySqlHelper = new DBMySqlHelper(server, dbName, userId, passWord, kou);
                MessageBox.Show("MySql连接成功！");
                string sql = "SELECT * FROM TB_TEST_METHOD";
                DataTable dt = MySqlHelper.GetDataTable(sql, "TB_TEST_METHOD");
                string id = dt.Rows[0]["NAME"].ToString();
                MessageBox.Show(id);
            }
            catch (Exception)
            {
                MessageBox.Show("MySql连接失败！");
                return;
            }
        }

        #region Active
        public override void Active()
        {
            base.Active();
        }
        #endregion

        #region InitData
        /// <summary>
        /// 用于执行控件数据初始化；
        /// </summary>
        public void InitData()
        {
            string sql = "SELECT * FROM BASAS WHERE AS01 = '" + PO + "' AND AS02 = '" + Process + "'";
            DataTable dt = null;// m_CBiz.ExecQuerySQL(sql, "BASAS");

            if (listAssembleProduct != null && listAssembleProduct.Count > 0)
            {
                listAssembleProduct.Clear();
            }
            foreach (DataRow dr in dt.Rows)
            {
                AssembleProductInfo api = new AssembleProductInfo(dr["RecId"].ToString(), dr["AS08"].ToString(), dr["AS09"].ToString());
                api.BarString = "";
                listAssembleProduct.Add(api);
            }
            AssembleProductInfo apiProcess = new AssembleProductInfo(
                Process,"ThisProcess",""
                );
            listAssembleProduct.Add(apiProcess);
        }
        #endregion

        #region CScanResult ExecScanBarOperate
        public CScanResult ExecScanBarOperate(string BarString)
        {
            CScanResult csr = new CScanResult();

            csr.BarString = BarString;
            csr.Result = "END";
            csr.Remark = "该产品在本工序生产完成...";
            return csr;
        }
        #endregion
    }
}
