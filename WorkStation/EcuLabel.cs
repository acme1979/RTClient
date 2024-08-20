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

namespace WorkStation
{
    public partial class EcuLabel : CUserControl, ICommonWorkStationCtl
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
        /// MES条码
        /// </summary>
        string MesSn = "";
        /// <summary>
        ///声音控制
        /// </summary>
        SoundPlayerHelp sound = new SoundPlayerHelp();

        public EcuLabel()
        {
            InitializeComponent();

            tbScanData.KeyDown += new KeyEventHandler(tbScanData_KeyDown);
            this.timerMusic.Tick += new EventHandler(timerMusic_Tick);
            btnXG.Click += new EventHandler(btnXG_Click);
        }

        #region btnXG_Click
        private void btnXG_Click(object sender, EventArgs e)
        {
            if ("修改".Equals(btnXG.Text))
            {
                btnXG.Text = "确认";
            }
            else
            {
                int cLen = 0;
                int sLen = 0;
                int.TryParse(txtCustSnLen.Text, out cLen);
                int.TryParse(txtSnLen.Text, out sLen);
                if (cLen <= 0)
                {
                    MessageBox.Show("客户条码不能小于0");
                    return;
                }
                if (cLen > 100)
                {
                    MessageBox.Show("客户条码不能大于100");
                    return;
                }
                if (sLen <= 0)
                {
                    MessageBox.Show("产品条码不能小于0");
                    return;
                }
                if (sLen > 100)
                {
                    MessageBox.Show("产品条码不能大于100");
                    return;
                }
                btnXG.Text = "修改";

                IniFileHelper.WriteIniData("workStationNum", WorkStation + "CustSnLen", txtCustSnLen.Text, wsIniPath);
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "SnLen", txtSnLen.Text, wsIniPath);
            }
            txtCustSnLen.ReadOnly = !txtCustSnLen.ReadOnly;
            txtSnLen.ReadOnly = !txtSnLen.ReadOnly;
        }
        #endregion

        #region InitData
        public void InitData()
        {
            string tcs = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "CustSnLen", "", wsIniPath);
            string ts = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "SnLen", "", wsIniPath);
            if ("".Equals(tcs))
            {
                tcs = "0";
            }
            if ("".Equals(ts))
            {
                ts = "0";
            }
            txtCustSnLen.Text = tcs;
            txtCustSnLen.ReadOnly = true;
            txtSnLen.Text = ts;
            txtSnLen.ReadOnly = true;
        }
        #endregion

        #region 回车事件
        /// <summary>
        /// 回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbScanData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbScanData.Text.Trim() != "")
                {
                    ScanDataOperate(tbScanData.Text.Trim());
                    tbScanData.Text = "";
                    tbScanData.Focus();
                }
            }
        }
        #endregion

        #region ScanDataOperate
        private void ScanDataOperate(string barString)
        {
            #region 初始化
            if ("INIT".Equals(barString.ToUpper()))
            {
                MesSn = "";
                lblTS.Text = "提示：请扫描MES条码";
                lblMsg("OK", "OK：初始化成功，请扫描MES条码");
                return;
            }
            #endregion
            #region 长度确认
            if ("确认".Equals(btnXG.Text))
            {
                lblMsg("NG", "NG：请确认客户条码长度");
                return;
            }
            #endregion
            int cLen = 0;
            int.TryParse(txtCustSnLen.Text, out cLen);
            int sLen = 0;
            int.TryParse(txtSnLen.Text, out sLen);
            if (cLen <= 0)
            {
                lblMsg("NG", "NG：请设置客户条码长度");
                return;
            }
            if (sLen <= 0)
            {
                lblMsg("NG", "NG：请设置产品条码长度");
                return;
            }

            string sql = "SELECT WT_ERROR_FLAG FROM T_WIP_TRACKING WHERE WT_SN = '" + barString + "' AND DATA_AUTH = '" + data_auth + "'";
            #region MES条码校验
            if ("".Equals(MesSn))
            {
                if (!checkBox1.Checked) 
                {
                    DataTable tracking = dbHelper.GetDataTable(sql, "T_WIP_TRACKING");
                    if (tracking.Rows.Count <= 0)
                    {
                        lblMsg("NG", "NG：该条码MES不存在生产记录");
                        return;
                    }
                    if (!"0".Equals(tracking.Rows[0]["WT_ERROR_FLAG"].ToString()))
                    {
                        lblMsg("NG", "NG：该条码品质不是良品");
                        return;
                    }
                    sql = "SELECT ID FROM T_WIP_ECU_TB WHERE ET_MES_SN = '" + barString + "'";
                    tracking = dbHelper.GetDataTable(sql, "T_WIP_ECU_TB");
                    if (tracking.Rows.Count > 0)
                    {
                        lblMsg("NG", "NG：该MES条码已有绑定记录");
                        return;
                    }
                }
                else
                {
                    if (barString.Length != sLen)
                    {
                        lblMsg("NG", "NG：该产品条码长度不符合");
                        return;
                    }
                    sql = "SELECT ID FROM T_WIP_ECU_TB WHERE ET_MES_SN = '" + barString + "'";
                    DataTable tracking = dbHelper.GetDataTable(sql, "T_WIP_ECU_TB");
                    if (tracking.Rows.Count > 0)
                    {
                        lblMsg("NG", "NG：该MES条码已有绑定记录");
                        return;
                    }
                }

                MesSn = barString;
                lblMsg("OK", "OK：请扫描客户条码");
                lblTS.Text = "提示：请扫描客户条码";
            }
            #endregion
            #region 客户条码校验
            else
            {
                if (barString.Length != cLen)
                {
                    lblMsg("NG", "NG：该客户条码长度不符合");
                    return;
                }
                DataTable dt = dbHelper.GetDataTable(sql, "T_WIP_TRACKING");
                if (dt.Rows.Count > 0)
                {
                    lblMsg("NG", "NG：该客户条码MES存在生产记录，不能作为客户条码");
                    return;
                }
                sql = "SELECT ID FROM T_WIP_ECU_TB WHERE ET_CUST_SN = '" + barString + "'";
                dt = dbHelper.GetDataTable(sql, "T_WIP_ECU_TB");
                if (dt.Rows.Count > 0)
                {
                    lblMsg("NG", "NG：该客户条码已有绑定记录");
                    return;
                }

                sql = "SELECT * FROM T_WIP_TRACKING WHERE WT_SN = '" + MesSn + "' AND DATA_AUTH = '" + data_auth + "'";
                dt = dbHelper.GetDataTable(sql, "T_WIP_ECU_TB");
                if (checkBox1.Checked)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("INSERT INTO T_WIP_ECU_TB(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ");
                        sb.Append("ET_MES_SN,ET_CUST_SN,ET_MO_NUMBER,ET_MS_PROCESS,ET_MS_STATION,ET_ITEM_CODE,ET_PROJECT_ID) VALUES ");
                        sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                        sb.Append(MesSn + "','" + barString + "','','','','','')");
                        dbHelper.ExecuteSql(sb.ToString());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("数据保存异常");
                        lblMsg("NG", "NG：数据保存异常,请扫描MES条码");
                        MesSn = "";
                        return;
                    }
                }
                else
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("INSERT INTO T_WIP_ECU_TB(ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ");
                        sb.Append("ET_MES_SN,ET_CUST_SN,ET_MO_NUMBER,ET_MS_PROCESS,ET_MS_STATION,ET_ITEM_CODE,ET_PROJECT_ID) VALUES ");
                        sb.Append("('" + Guid.NewGuid().ToString() + "','" + m_UsrCode + "',sysdate,'" + data_auth + "','");
                        sb.Append(MesSn + "','" + barString + "','" + dt.Rows[0]["WT_MO_NUMBER"] + "','");
                        sb.Append(dt.Rows[0]["WT_GROUP_CODE"] + "','" + dt.Rows[0]["WT_WORK_STATION"] + "','");
                        sb.Append(dt.Rows[0]["WT_MODEL_CODE"] + "','" + dt.Rows[0]["WT_PROJECT_ID"] + "')");
                        dbHelper.ExecuteSql(sb.ToString());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("数据保存异常");
                        lblMsg("NG", "NG：数据保存异常,请扫描MES条码");
                        MesSn = "";
                        return;
                    }
                }


                dataGridView1.Rows.Insert(0, 1);
                DataGridViewRow dgvRow = dataGridView1.Rows[0];
                dgvRow.Cells["SN"].Value = dataGridView1.Rows.Count;
                dgvRow.Cells["MES_SN"].Value = MesSn;
                dgvRow.Cells["CUST_SN"].Value = barString;
                lblMsg("OK", "OK：绑定成功，请扫描MES条码");
                MesSn = "";
                lblTS.Text = "提示：请扫描MES条码";
            }
            #endregion

            txtOldSn.Text = barString;

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
