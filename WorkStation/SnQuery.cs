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

namespace WorkStation
{
    public partial class SnQuery : CUserControl, ICommonWorkStationCtl
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
        public SnQuery()
        {
            InitializeComponent();
            label2.Text = "";
            label2.BackColor = SystemColors.Control;
            tbQuery.KeyDown += new KeyEventHandler(tbQuery_KeyDown);
            timerMusic.Tick += new EventHandler(timerMusic_Tick);
        }

        #region InitData
        public void InitData()
        {

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

        #region 回车事件
        private void tbQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ScanSnQuery(tbQuery.Text.Trim());
                tbQuery.Focus();
                tbQuery.SelectAll();
            }
        }
        #endregion

        #region 回车业务逻辑
        private void ScanSnQuery(string sn)
        {
            if ("".Equals(sn))
            {
                lblMsg("NG", "NG：产品SN输入不能为空");
                return;
            }
            DataTable dt01 = SelectSnInfo(sn);
            if (dt01.Rows.Count < 1)
            {
                lblMsg("NG", "NG：输入的SN无任何信息");
                return;
            }
            string snstatus = "";
            if ("0".Equals(dt01.Rows[0]["WT_ERROR_FLAG"].ToString()))
            {
                snstatus = "OK";
                tbError.Text = "无";
            }
            else
            {
                snstatus = "NG";
                DataTable dt02 = SelectErrorInfo(dt01.Rows[0]["WT_SN"].ToString(), dt01.Rows[0]["WT_GROUP_CODE"].ToString());
                if (dt02.Rows.Count < 1)
                {
                    lblMsg("NG", "NG：该产品无不良维修记录");
                    return;
                }
                else
                {
                    tbError.Text = dt02.Rows[0]["ERROR_INFO"].ToString();
                }
            }

            tbMo.Text = dt01.Rows[0]["WT_MO_NUMBER"].ToString();
            tbModel.Text = dt01.Rows[0]["WT_MODEL_CODE"].ToString();
            tbGroup.Text = dt01.Rows[0]["GROUP_NAME"].ToString();
            tbBackGroup.Text = dt01.Rows[0]["BACK_GROUP"].ToString();
            tbInTime.Text = dt01.Rows[0]["WT_IN_TIME"].ToString();
            tbFinishFlag.Text = dt01.Rows[0]["FINISH_FLAG"].ToString();
            refreshStatus(snstatus);
            lblMsg("OK", "OK：请输入产品SN");
        }
        #endregion

        #region 产品状态
        private void refreshStatus(string status)
        {
            if ("OK".Equals(status.ToUpper()))
            {
                label2.Text = "良品";
                label2.BackColor = Color.Lime;
            }
            else
            {
                label2.Text = "不良";
                label2.BackColor = Color.Red;
            }
        }
        #endregion

        #region lblMsg
        private void lblMsg(string status, string msg)
        {
            lblRltState.Text = msg;
            if ("OK".Equals(status))
            {
                lblRltState.BackColor = Color.Green;
                sound.passPlayer.PlayLooping();//播放通过提示音
            }
            else
            {
                lblRltState.BackColor = Color.Red;
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
        private DataTable SelectSnInfo(string sn)
        {
            string sql = string.Format(@"SELECT WT.WT_SN ,
                                         WT.WT_MO_NUMBER ,
                                         WT.WT_MODEL_CODE ,
                                         WT.WT_GROUP_CODE ,
                                         WT.WT_GROUP_CODE || '/' || CG1.GROUP_NAME GROUP_NAME ,
                                         WT.WT_BACK_GROUP || '/' || CG2.GROUP_NAME BACK_GROUP ,
                                         WT.WT_IN_TIME ,
                                         DECODE(WT.WT_FINISH_FLAG , 'Y' , '是' , 'N' , '否') FINISH_FLAG ,
                                         WT.WT_ERROR_FLAG
                                         FROM T_WIP_TRACKING WT
                                         LEFT JOIN T_CO_GROUP CG1
                                         ON WT.WT_GROUP_CODE = CG1.GROUP_CODE
                                         LEFT JOIN T_CO_GROUP CG2
                                         ON WT.WT_BACK_GROUP = CG2.GROUP_CODE
                                         WHERE WT.WT_SN = '{0}' AND ROWNUM = 1", sn);
            DataTable dt = dbHelper.GetDataTable(sql, "T_WIP_TRACKING");
            return dt;
        }

        private DataTable SelectErrorInfo(string sn, string groupname)
        {
            string sql = string.Format(@"SELECT WE.WE_ERROR_CODE || '/' || CEC.CEC_NAME ERROR_INFO
                                         FROM T_WIP_ERROR WE
                                         LEFT JOIN T_CO_ERROR_CODE CEC
                                         ON WE.WE_ERROR_CODE = CEC.CEC_CODE
                                         WHERE WE.WE_SN = '{0}'
                                         AND WE.WE_TEST_GROUP = '{1}'", sn, groupname);
            DataTable dt = dbHelper.GetDataTable(sql, "T_WIP_ERROR");
            return dt;
        }
        #endregion
    }
}
