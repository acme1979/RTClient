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
    public partial class Packing : CUserControl, ICommonWorkStationCtl
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
        /// <summary>
        /// 容器条码
        /// </summary>
        string BoxSn = "";
        /// <summary>
        /// 产品条码
        /// </summary>
        string MesSn = "";
        /// <summary>
        /// 操作状态（0-放入、1-取出）
        /// </summary>
        string OperateType = "";
        /// <summary>
        /// 产品料号
        /// </summary>
        string ModelCode = "";
        /// <summary>
        /// 产品名称
        /// </summary>
        string ModelName = "";
        /// <summary>
        /// 包装目标数量
        /// </summary>
        int PackTargetNum = 0;
        /// <summary>
        /// 包装实际数量
        /// </summary>
        int PackNum = 0;
        /// <summary>
        /// 产品SN类型(0-产品、1-容器、2-组件)
        /// </summary>
        string SnType = "";

        public Packing()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmPacking_Load);
            this.btnEdit.Click += new EventHandler(btnEdit_Click);
            this.tbScanData.KeyDown += new KeyEventHandler(tbScanData_KeyDown);
            this.timerMusic.Tick += new EventHandler(timerMusic_Tick);
        }

        #region Form_Load
        private void frmPacking_Load(object sender, EventArgs e)
        {
            ReStart();
        }
        #endregion

        #region 修改点击
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if ("修改".Equals(btnEdit.Text))
            {
                btnEdit.Text = "确认";
            }
            else
            {
                btnEdit.Text = "修改";
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "BoxSnLen", nudBoxsnLen.Value.ToString(), wsIniPath);
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "BoxSnCheckStr", tbBoxSnCheckStr.Text, wsIniPath);
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "BoxSnStart", nudBoxsnStart.Value.ToString(), wsIniPath);
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "SnLen", nudSnLen.Value.ToString(), wsIniPath);
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "SnCheckStr", tbSnCheckStr.Text, wsIniPath);
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "SnStart", nudSnStart.Value.ToString(), wsIniPath);
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "PackNum", nudPackNum.Value.ToString(), wsIniPath);
            }
            //nudBoxsnLen.ReadOnly = !nudBoxsnLen.ReadOnly;
            nudBoxsnLen.Enabled = !nudBoxsnLen.Enabled;
            tbBoxSnCheckStr.Enabled = !tbBoxSnCheckStr.Enabled;
            nudBoxsnStart.Enabled = !nudBoxsnStart.Enabled;
            nudSnLen.Enabled = !nudSnLen.Enabled;
            tbSnCheckStr.Enabled = !tbSnCheckStr.Enabled;
            nudSnStart.Enabled = !nudSnStart.Enabled;
            nudPackNum.Enabled = !nudPackNum.Enabled;
        }
        #endregion

        #region 回车事件
        private void tbScanData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbScanData.Text.Trim() != "")
                {
                    ScanDataOperate(tbScanData.Text.Trim());
                    tbScanData.Focus();
                    tbScanData.SelectAll();
                }
            }
        }
        #endregion

        #region 操作保存
        private void ScanDataOperate(string scandata)
        {
            string res = "OK";
            string otn = "";
            #region 初始化
            if ("UNDO".Equals(scandata.ToUpper()))
            {
                otn = "UNDO";
                ReStart();
                ReFresh(scandata, res, otn);
                return;
            }
            #endregion

            #region 放入容器
            if ("INPUT".Equals(scandata.ToUpper()))
            {
                OperateType = "0";
                otn = "INPUT";
                lblOperateType.Text = "放入产品";
                BoxSn = "";
                MesSn = "";
                lblMsg("OK", "OK：切换为放入产品状态,请输入容器SN");
                ReFresh(scandata, res, otn);
                return;
            }
            #endregion

            #region 取出容器
            if ("OUTPUT".Equals(scandata.ToUpper()))
            {
                OperateType = "1";
                otn = "OUTPUT";
                lblOperateType.Text = "取出产品";
                BoxSn = "";
                MesSn = "";
                lblMsg("OK", "OK：切换为取出产品状态，请输入产品SN或容器SN");
                ReFresh(scandata, res, otn);
                return;
            }
            #endregion

            if ("确认".Equals(btnEdit.Text)) 
            {
                lblMsg("NG", "NG：请保存设置");
                return;
            }

            //校验操作类型
            if ("".Equals(OperateType))
            {
                lblMsg("NG", "NG：请输入操作指令");
                return;
            }
            if ("0".Equals(OperateType))
            {
                otn = "INPUT";
            }
            if ("1".Equals(OperateType))
            {
                otn = "OUTPUT";
            }
            //判断容器SN是否为空
            if (("".Equals(BoxSn) && "0".Equals(OperateType)) || ("1".Equals(OperateType) && cbOutAllBox.Checked)) 
            {
                res = BoxSnCheck(scandata);
                if ("OK".Equals(res))
                {
                    BoxSn = scandata;
                    //判断作业模式（放入、取出）
                    if ("0".Equals(OperateType))
                    {
                        lblMsg("OK", "OK：请输入产品SN");
                    }
                    if ("1".Equals(OperateType))
                    {
                        Hashtable ht = new Hashtable();
                        DataTable dt01 = SelectPackSnInfoByBoxSn(scandata, data_auth);
                        //判断是否产生FQC单据
                        if (cbFqc.Checked == true)
                        {
                            for (int i = 0; i < dt01.Rows.Count; i++)
                            {
                                string wipsn = "";
                                string packsn = "";
                                string str01 = dt01.Rows[i]["PSI_SN"].ToString();
                                DataTable dt02 = SelectWipTrackingBySn(str01, data_auth);
                                DataTable dt03 = SelectWipKpInfoByKeypSn(str01, data_auth);
                                if (dt02.Rows.Count < 1 && dt03.Rows.Count > 0)
                                {
                                    wipsn = dt03.Rows[0]["WKI_SN"].ToString();
                                    packsn = str01;
                                }
                                wipsn = str01;
                                packsn = str01;
                                ht = FqcInput("1", "7", wipsn, dt01.Rows[0]["PSI_MODEL_CODE"].ToString(), dt01.Rows[0]["PSI_MO_NUMBER"].ToString(), "1", m_UsrCode);
                                res = ht["RES"].ToString();
                                if (!("OK".Equals(res)))
                                {
                                    lblMsg("NG", res);
                                    ReFresh(scandata, res, otn);
                                    return;
                                }
                                DeletePackSnInfoBySn(packsn, data_auth);
                            }
                        }
                        else
                        {
                            DeletePackSnInfoByBoxSn(scandata, data_auth);
                        }
                        lblMsg("OK", "OK：请输入容器SN");
                    }
                }
                else
                {
                    lblMsg("NG", res);
                }
                ReFresh(scandata, res, otn);
            }
            else
            {
                res = SnCheck(scandata, BoxSn);
                //校验SN
                if ("OK".Equals(res))
                {
                    MesSn = scandata;
                    //判断作业模式（放入、取出）
                    if("0".Equals(OperateType))
                    {
                        if ("0".Equals(SnType))
                        {
                            InsertPackSnInfoByWipTracking(MesSn, m_WorkStation, m_ProductLine, BoxSn, m_UsrCode, data_auth);
                        }
                        if ("1".Equals(SnType))
                        {
                            InsertPackSnInfoByPackSnInfo(MesSn, m_WorkStation, m_ProductLine, BoxSn, m_UsrCode, data_auth);
                        }
                        if ("2".Equals(SnType))
                        {
                            InsertPackSnInfoByWipKeypInfo(MesSn, m_WorkStation, m_ProductLine, BoxSn, m_UsrCode, data_auth);
                        }
                        DataTable dt01 = SelectPackSnInfoByBoxSn(BoxSn, data_auth);
                        if (dt01.Rows.Count >= nudPackNum.Value)
                        {
                            lblMsg("OK", "OK：当前容器已装满，请输入新容器SN");
                        }
                        else
                        {
                            lblMsg("OK", "OK：请输入产品SN");
                        }
                    }
                    if("1".Equals(OperateType))
                    {
                        Hashtable ht = new Hashtable();
                        DataTable dt01 = SelectPackSnInfoBySn(MesSn, data_auth);
                        DataTable dt02 = SelectWipTrackingBySn(MesSn, data_auth);
                        DataTable dt03 = SelectWipKpInfoByKeypSn(MesSn, data_auth);
                        //判断是否产生FQC单据
                        if (cbFqc.Checked == true)
                        {
                            if ((dt02.Rows.Count < 1) && (dt03.Rows.Count > 0)) 
                            {
                                MesSn = dt03.Rows[0]["WKI_SN"].ToString();
                            }
                            ht = FqcInput("1", "7", MesSn, dt01.Rows[0]["PSI_MODEL_CODE"].ToString(), dt01.Rows[0]["PSI_MO_NUMBER"].ToString(), "1", m_UsrCode);
                            res = ht["RES"].ToString();
                            string docnum = ht["M_INSPECT_SN"].ToString();
                            if (!("OK".Equals(res)))
                            {
                                lblMsg("NG", res);
                                ReFresh(scandata, res, otn);
                                return;
                            }
                        }
                        DeletePackSnInfoBySn(scandata, data_auth);
                        lblMsg("OK", "OK：请输入产品SN");
                    }
                }
                else
                {
                    lblMsg("NG", res);
                }
            }
            ReFresh(scandata, res, otn);
        }
        #endregion

        #region 校验容器SN
        private string BoxSnCheck(string boxsn)
        {
            string res = "OK";
            //if ("".Equals(tbBoxSnCheckStr.Text))
            //{
            //    if (boxsn.Length != nudBoxsnLen.Value)
            //    {
            //        res = "NG：容器SN编码长度校验NG（长度" + boxsn.Length.ToString() + "）";
            //        return res;
            //    }
            //}
            //else
            //{
            //    if (!((boxsn.Length == nudBoxsnLen.Value) && (boxsn.Substring(Convert.ToInt32((nudBoxsnStart.Value - 1).ToString()), tbBoxSnCheckStr.Text.Length) == tbBoxSnCheckStr.Text)))  
            //    {
            //        res = "NG：容器SN编码校验字符不通过";
            //        return res;
            //    }
            //}
            if (boxsn.Length != nudBoxsnLen.Value)
            {
                res = "NG：容器SN编码长度校验错误（长度" + boxsn.Length.ToString() + "）";
                return res;
            }
            if (!("".Equals(tbBoxSnCheckStr.Text)) && !(boxsn.Substring(Convert.ToInt32((nudBoxsnStart.Value - 1).ToString()), tbBoxSnCheckStr.Text.Length) == tbBoxSnCheckStr.Text))
            {
                res = "NG：容器SN编码校验字符错误";
                return res;
            }
            DataTable dt01 = SelectPackSnInfoByBoxSn(boxsn, data_auth);
            if (dt01.Rows.Count >= nudPackNum.Value && "0".Equals(OperateType)) 
            {
                res = "NG：容器已装满";
                return res;
            }
            if (dt01.Rows.Count < 1 && "1".Equals(OperateType))
            {
                res = "NG：容器无包装信息";
                return res;
            }
            //if (dt01.Rows.Count < 1 && "1".Equals(OperateType))
            //{
            //    res = "NG：容器不存在";
            //    return res;
            //}
            return res;
        }
        #endregion

        #region 校验产品SN
        private string SnCheck(string sn, string boxsn)
        {
            string res = "OK";
            if ("".Equals(boxsn) && "0".Equals(OperateType)) 
            {
                res = "NG：容器SN为空，请输入容器SN";
                return res;
            }
            //if ("".Equals(tbSnCheckStr.Text))
            //{
            //    if (sn.Length != nudSnLen.Value)
            //    {
            //        res = "NG：产品SN编码长度校验NG（长度" + sn.Length.ToString() + "）";
            //        return res;
            //    }
            //}
            //if (!((sn.Length == nudSnLen.Value) && (sn.Substring(Convert.ToInt32((nudSnStart.Value - 1).ToString()), tbSnCheckStr.Text.Length) == tbSnCheckStr.Text))) 
            //{
            //    res = "NG：产品SN编码校验字符NG";
            //    return res;
            //}
            if (sn.Length != nudSnLen.Value)
            {
                res = "NG：产品SN编码长度校验错误（长度" + sn.Length.ToString() + "）";
                return res;
            }
            if (!("".Equals(tbSnCheckStr.Text)) && !(sn.Substring(Convert.ToInt32((nudSnStart.Value - 1).ToString()), tbSnCheckStr.Text.Length) == tbSnCheckStr.Text)) 
            {
                res = "NG：产品SN编码校验字符错误";
                return res;
            }
            DataTable dt01 = SelectWipTrackingBySn(sn, data_auth);
            DataTable dt02 = SelectPackSnInfoByBoxSn(sn, data_auth);
            DataTable dt05 = SelectWipKpInfoByKeypSn(sn, data_auth);
            if (dt01.Rows.Count < 1 && dt02.Rows.Count < 1 && dt05.Rows.Count < 1 && "0".Equals(OperateType)) 
            {
                res = "NG：产品不满足包装条件";
                return res;
            }
            if (!("".Equals(ModelCode)) && "0".Equals(OperateType)) 
            {
                if (dt01.Rows.Count > 0) 
                {
                    if (dt01.Rows[0]["WT_MODEL_CODE"].ToString() != ModelCode)
                    {
                        res = "NG：产品与容器机种不一致";
                        return res;
                    }
                }
                if (dt02.Rows.Count > 0)
                {
                    if (dt02.Rows[0]["PSI_MODEL_CODE"].ToString() != ModelCode)
                    {
                        res = "NG：产品与容器机种不一致";
                        return res;
                    }
                }
                if (dt05.Rows.Count > 0)
                {
                    if (dt05.Rows[0]["WT_MODEL_CODE"].ToString() != ModelCode)
                    {
                        res = "NG：产品与容器机种不一致";
                        return res;
                    }
                }
                //if ((dt01.Rows[0]["WT_MODEL_CODE"].ToString() != ModelCode) || (dt02.Rows[0]["PSI_MODEL_CODE"].ToString() != ModelCode)) 
                //{
                //    res = "NG：产品与容器机种不一致";
                //    return res;
                //}
            }
            DataTable dt03 = SelectPackSnInfoBySn(sn, data_auth);
            if (dt03.Rows.Count > 0 && "0".Equals(OperateType)) 
            {
                res = "NG：产品已有包装记录(" + dt03.Rows[0]["PSI_CONTAINER_SN"].ToString() + ")";
                return res;
            }
            if (dt03.Rows.Count < 1 && "1".Equals(OperateType)) 
            {
                res = "NG：产品无包装记录";
                return res;
            }
            if (dt03.Rows.Count > 0 && "1".Equals(OperateType))
            {
                BoxSn = dt03.Rows[0]["PSI_CONTAINER_SN"].ToString();
            }
            DataTable dt04 = SelectPackSnInfoByBoxSn(boxsn, data_auth);
            int packnum = dt04.Rows.Count;
            if (packnum >= nudPackNum.Value && "0".Equals(OperateType)) 
            {
                res = "NG：容器已装满";
                return res;
            }
            if (dt01.Rows.Count > 0)
            {
                SnType = "0";
            }
            if (dt02.Rows.Count > 0)
            {
                SnType = "1";
            }
            if (dt05.Rows.Count > 0)
            {
                SnType = "2";
            }
            return res;
        }
        #endregion

        #region 重置
        private void ReStart()
        {
            BoxSn = "";
            MesSn = "";
            OperateType = "";
            SnType = "";
            ModelCode = "";
            ModelName = "";
            lblOperateType.Text = "";
            lblBoxSn.Text = "";
            lblModelCode.Text = "";
            lblModelName.Text = "";
            lblPackNum.Text = "0";
            DataTable dt01 = SelectPackSnInfoByWorkstation(m_WorkStation, data_auth);
            dataGridView1.DataSource = dt01;
            lblMsg("OK", "OK：初始化成功，请输入操作指令");
        }
        #endregion

        #region 刷新
        private void ReFresh(string instr, string res, string otn)
        {
            SaveLogs(UsrCode, DATA_AUTH, res, "\"SCAN_DATA\":" + "\"" + instr + "\"" + "," + "\"MES_SN\":" + "\"" + MesSn + "\"" + "," + "\"BOX_SN\":" + "\"" + BoxSn + "\"" + "," + "\"WORK_STATION\":" + "\"" + WorkStation + "\"" + "," + "\"PACK_NUM\":" + "\"" + nudPackNum.Value.ToString() + "\"" + "," + "\"CMD\":" + "\"" + otn + "\""+"," + "\"EMP\":" + "\"" + UsrCode + "\"", "包装模块", "true");
            DataTable dt01 = SelectPackSnInfoByBoxSn(BoxSn, data_auth);
            PackNum = dt01.Rows.Count;
            if (PackNum >= nudPackNum.Value && "0".Equals(OperateType)) 
            {
                BoxSn = "";
                MesSn = "";
                PackNum = 0;
            }
            if (PackNum < 1 && "1".Equals(OperateType))
            {
                BoxSn = "";
                MesSn = "";
                PackNum = 0;
            }
            lblBoxSn.Text = BoxSn;
            if (PackNum > 0)
            {
                ModelCode = dt01.Rows[0]["PSI_MODEL_CODE"].ToString();
                ModelName = dt01.Rows[0]["CI_ITEM_NAME"].ToString();
                lblModelCode.Text = ModelCode;
                lblModelName.Text = ModelName;
            }
            else
            {
                lblModelCode.Text = "";
                lblModelName.Text = "";
            }
            lblPackNum.Text = PackNum.ToString();
            DataTable dt02 = SelectPackSnInfoByWorkstation(m_WorkStation, data_auth);
            dataGridView1.DataSource = dt02;
        }
        #endregion

        #region SQL
        private void SaveLogs(string usrcode, string dataauth, string message, string datainfo, string funname, string resflag)
        {
            string sql = string.Format(@"INSERT INTO T_XC_SYNC_LOG 
                                        (ID, CREATE_USER, CREATE_TIME, DATA_AUTH, ERRORDATE, ERRORMESSAGE, LOG_DATA_INFO, LOG_FUNCTION_NAME, RESULT_FLAG)
                                        VALUES(RAWTOHEX(SYS_GUID()) , '{0}' , SYSDATE , '{1}' , SYSDATE , '{2}' , '{3}' , '{4}' , '{5}')"
                                        , usrcode, dataauth, message, datainfo, funname, resflag);
            dbHelper.ExecuteSql(sql);
        }

        private DataTable SelectWipTrackingBySn(string sn , string dataauth)
        {
            string sql = string.Format(@"SELECT * FROM T_WIP_TRACKING WT
                                         WHERE WT.WT_SN = '{0}' AND WT.DATA_AUTH = '{1}' AND WT.WT_FINISH_FLAG = 'Y'", sn, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_WIP_TRACKING");
            return dt;
        }

        private DataTable SelectWipKpInfoByKeypSn(string keypsn, string dataauth)
        {
            string sql = string.Format(@"SELECT * FROM T_WIP_KEYP_INFO WEI
                                         LEFT JOIN T_WIP_TRACKING WT
                                         ON WEI.WKI_SN = WT.WT_SN
                                         WHERE WEI.WKI_KEYP_SN = '{0}' AND WEI.DATA_AUTH = '{1}' AND WT.WT_FINISH_FLAG = 'Y'"
                                         , keypsn, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_WIP_TRACKING");
            return dt;
        }

        private DataTable SelectPackSnInfoByBoxSn(string boxsn, string dataauth)
        {
            string sql = string.Format(@"SELECT PSI.* , CI.CI_ITEM_NAME FROM T_PACK_SN_INFO PSI
                                         LEFT JOIN T_CO_ITEM CI
                                         ON PSI.PSI_MODEL_CODE = CI.CI_ITEM_CODE
                                         WHERE PSI.PSI_CONTAINER_SN = '{0}' AND PSI.DATA_AUTH = '{1}'", boxsn, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_PACK_SN_INFO");
            return dt;
        }

        private DataTable SelectPackSnInfoByWorkstation(string workstation, string dataauth)
        {
            string sql = string.Format(@"SELECT PSI.PSI_SN 产品序列号 , PSI.PSI_CONTAINER_SN 所属容器 , PSI.PSI_MO_NUMBER 制令单号 , PSI.PSI_MODEL_CODE 产品料号 , CI.CI_ITEM_NAME 产品名称 , PSI.PSI_TIME 包装时间 FROM T_PACK_SN_INFO PSI
                                         LEFT JOIN T_CO_ITEM CI
                                         ON PSI.PSI_MODEL_CODE = CI.CI_ITEM_CODE
                                         WHERE PSI.PSI_WORK_STATION = '{0}' AND PSI.DATA_AUTH = '{1}' AND TRUNC(PSI.PSI_TIME) = TRUNC(SYSDATE)
                                         ORDER BY PSI.PSI_TIME DESC", workstation, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_PACK_SN_INFO");
            return dt;
        }

        private DataTable SelectPackSnInfoBySn(string sn, string dataauth)
        {
            string sql = string.Format(@"SELECT PSI.* , CI.CI_ITEM_NAME FROM T_PACK_SN_INFO PSI
                                         LEFT JOIN T_CO_ITEM CI
                                         ON PSI.PSI_MODEL_CODE = CI.CI_ITEM_CODE
                                         WHERE PSI.PSI_SN = '{0}' AND PSI.DATA_AUTH = '{1}'", sn, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_PACK_SN_INFO");
            return dt;
        }

        private void InsertPackSnInfoByWipTracking(string sn , string workstation , string areasn , string boxsn , string emp , string dataauth)
        {
            string sql = string.Format(@"INSERT INTO T_PACK_SN_INFO PSI
                                         (PSI.ID , PSI.PSI_SN , PSI.PSI_MO_NUMBER , PSI.PSI_PROJECT_ID , PSI.PSI_PRODUCT_STEP , PSI.PSI_WORK_STATION ,
                                         PSI.PSI_AREA_SN , PSI.PSI_PROCESS_FACE , PSI.PSI_CONTAINER_SN , PSI.PSI_MODEL_CODE ,
                                         PSI.PSI_SN_NUM , PSI.PSI_TIME , PSI.PSI_EMP , PSI.DATA_AUTH , PSI.PSI_LOT)
                                         SELECT F_C_GETNEWID , WT.WT_SN , WT.WT_MO_NUMBER , WT.WT_PROJECT_ID , '3' , '{1}' , '{2}' , '0' ,
                                         '{3}' , WT.WT_MODEL_CODE , WT.WT_NUM , SYSDATE , '{4}' , '{5}' , WT.WT_LOT
                                         FROM T_WIP_TRACKING WT
                                         WHERE WT.WT_SN = '{0}' AND WT.DATA_AUTH = '{5}'"
                                        , sn , workstation , areasn , boxsn , emp , dataauth);
            dbHelper.ExecuteSql(sql);
        }

        private void InsertPackSnInfoByWipKeypInfo(string keypsn, string workstation, string areasn, string boxsn, string emp, string dataauth)
        {
            string sql = string.Format(@"INSERT INTO T_PACK_SN_INFO PSI
                                         (PSI.ID , PSI.PSI_SN , PSI.PSI_MO_NUMBER , PSI.PSI_PROJECT_ID , PSI.PSI_PRODUCT_STEP , PSI.PSI_WORK_STATION ,
                                         PSI.PSI_AREA_SN , PSI.PSI_PROCESS_FACE , PSI.PSI_CONTAINER_SN , PSI.PSI_MODEL_CODE ,
                                         PSI.PSI_SN_NUM , PSI.PSI_TIME , PSI.PSI_EMP , PSI.DATA_AUTH , PSI.PSI_LOT)
                                         SELECT F_C_GETNEWID , WEI.WKI_KEYP_SN , WT.WT_MO_NUMBER , WT.WT_PROJECT_ID , '3' , '{1}' , '{2}' ,
                                         '0' , '{3}' , WT.WT_MODEL_CODE , WT.WT_NUM , SYSDATE , '{4}' , '{5}' , WT.WT_LOT
                                         FROM T_WIP_KEYP_INFO WEI
                                         LEFT JOIN T_WIP_TRACKING WT
                                         ON WEI.WKI_SN = WT.WT_SN
                                         WHERE WEI.WKI_KEYP_SN = '{0}' AND WEI.DATA_AUTH = '{5}'"
                                        , keypsn, workstation, areasn, boxsn, emp, dataauth);
            dbHelper.ExecuteSql(sql);
        }

        private void InsertPackSnInfoByPackSnInfo(string sn, string workstation, string areasn, string boxsn, string emp, string dataauth)
        {
            string sql = string.Format(@"INSERT INTO T_PACK_SN_INFO PSI
                                         (PSI.ID , PSI.PSI_SN , PSI.PSI_MO_NUMBER , PSI.PSI_PROJECT_ID , PSI.PSI_PRODUCT_STEP , PSI.PSI_WORK_STATION ,
                                         PSI.PSI_AREA_SN , PSI.PSI_PROCESS_FACE , PSI.PSI_CONTAINER_SN , PSI.PSI_MODEL_CODE ,
                                         PSI.PSI_SN_NUM , PSI.PSI_TIME , PSI.PSI_EMP , PSI.DATA_AUTH , PSI.PSI_LOT)
                                         SELECT F_C_GETNEWID , PSI.PSI_CONTAINER_SN , PSI.PSI_MO_NUMBER , PSI.PSI_PROJECT_ID , '3' , '{1}' , '{2}' ,
                                         '0' , '{3}' , PSI.PSI_MODEL_CODE , PSI.PSI_SN_NUM , SYSDATE , '{4}' , '{5}' ,
                                         PSI.PSI_LOT
                                         FROM T_PACK_SN_INFO PSI
                                         WHERE PSI.PSI_CONTAINER_SN = '{0}' AND PSI.DATA_AUTH = '{5}'"
                                        , sn, workstation, areasn, boxsn, emp, dataauth);
            dbHelper.ExecuteSql(sql);
        }

        private void DeletePackSnInfoBySn(string sn, string dataauth)
        {
            string sql = string.Format(@"DELETE FROM T_PACK_SN_INFO PSI
                                         WHERE PSI.PSI_SN = '{0}' AND PSI.DATA_AUTH = '{1}'"
                                        , sn, dataauth);
            dbHelper.ExecuteSql(sql);
        }

        private void DeletePackSnInfoByBoxSn(string boxsn, string dataauth)
        {
            string sql = string.Format(@"DELETE FROM T_PACK_SN_INFO PSI
                                         WHERE PSI.PSI_CONTAINER_SN = '{0}' AND PSI.DATA_AUTH = '{1}'"
                                        , boxsn, dataauth);
            dbHelper.ExecuteSql(sql);
        }
        #endregion

        #region lblMsg
        private void lblMsg(string status, string msg)
        {
            lblResState.Text = msg;
            if ("OK".Equals(status))
            {
                lblResState.BackColor = Color.Green;
                sound.passPlayer.PlayLooping();//播放通过提示音
            }
            else
            {
                lblResState.BackColor = Color.Red;
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

        #region InitData
        public void InitData()
        {
            string bsl = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BoxSnLen", "", wsIniPath);
            string bscs = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BoxSnCheckStr", "", wsIniPath);
            string bss = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BoxSnStart", "", wsIniPath);
            string sl = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "SnLen", "", wsIniPath);
            string scs = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "SnCheckStr", "", wsIniPath);
            string ss = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "SnStart", "", wsIniPath);
            string pn = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "PackNum", "", wsIniPath);
            if ("".Equals(bsl.Trim()))
            {
                bsl = "0";
            }
            if ("".Equals(bscs.Trim()))
            {
                bscs = "";
            }
            if ("".Equals(bss.Trim()))
            {
                bss = "1";
            }
            if ("".Equals(sl.Trim()))
            {
                sl = "0";
            }
            if ("".Equals(scs.Trim()))
            {
                scs = "";
            }
            if ("".Equals(ss.Trim()))
            {
                ss = "1";
            }
            if ("".Equals(pn.Trim()))
            {
                pn = "0";
            }
            nudBoxsnLen.Enabled = false;
            nudBoxsnLen.Value = Convert.ToInt32(bsl);
            tbBoxSnCheckStr.Enabled = false;
            tbBoxSnCheckStr.Text = bscs;
            nudBoxsnStart.Enabled = false;
            nudBoxsnStart.Value = Convert.ToInt32(bss);
            nudSnLen.Enabled = false;
            nudSnLen.Value = Convert.ToInt32(sl);
            tbSnCheckStr.Enabled = false;
            tbSnCheckStr.Text = scs;
            nudSnStart.Enabled = false;
            nudSnStart.Value = Convert.ToInt32(ss);
            nudPackNum.Enabled = false;
            nudPackNum.Value = Convert.ToInt32(pn);
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

        #region FQC检验单生成
        /// <summary>
        /// 生产测试指令
        /// </summary>
        /// <param name="ListDBParameters"></param>
        /// <param name="SN">条码SN</param>
        /// <param name="ecCode">不良代码</param>
        /// <param name="ht"></param>
        /// <returns></returns>
        private Hashtable FqcInput(string qmstep,string cktype, string sn, string itemcode,string docnum,string num,string emp)
        {
            List<DBParameters> ListDBParameters = new List<DBParameters>();
            Hashtable ht = new Hashtable();
            //Hashtable htDmp = new Hashtable();

            dbHelper.AddDBParameters(ListDBParameters, "M_QM_STEP", qmstep, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_CKECK_TYPE", cktype, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_SEND_DEP", data_auth, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_SN", sn, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_ITEM_CODE", itemcode, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_DOC_NUM", docnum, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_NUM", num, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_EMP", emp, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_DATA_AUTH", data_auth, ParameterDirection.Input, valueTypes.STRING);

            dbHelper.AddDBParameters(ListDBParameters, "M_INSPECT_SN", null, ParameterDirection.Output, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "RES", null, ParameterDirection.Output, valueTypes.STRING);

            //htDmp.Add("M_DATA_AUTH", data_auth);
            //htDmp.Add("M_SN", SN);
            //htDmp.Add("M_WORK_STATIONID", WorkStation);
            //htDmp.Add("M_EC_STR", ecCode);
            //htDmp.Add("M_POINT_STR", "");
            //htDmp.Add("M_COUNT_STR", "");
            //htDmp.Add("M_NG_NUM", ngNum);
            //htDmp.Add("M_EMP", m_UsrCode);
            //htDmp.Add("M_FLAG", "");

            //string beginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                ht = dbHelper.htExecuteNonQuery(ListDBParameters, "P_C_SAVE_QM_BATCH");
            }
            catch (Exception err)
            {
                //htDmp.Add("RES", "NG:存储执行异常！");
                ht.Add("RES", "NG:存储执行异常！");
            }
            //htDmp.Add("FLOWCODE", ht["FLOWCODE"]);
            //htDmp.Add("RES", ht["RES"]);
            //string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //指令运行日志
            //try
            //{
            //    StringBuilder sb = new StringBuilder("BEGIN ");
            //    string tid = Guid.NewGuid().ToString();
            //    string tName = "[" + ip + "]:ICT " + SN;
            //    sb.Append("INSERT INTO T_FW_TASK_LOG (TFTL_ID,TFTL_NAME,TFTL_PROCE_DATE,TFTL_PROCE_EN_DATE,TFTL_DMP_ID,TFTL_IN_DATE)VALUES ('");
            //    sb.Append(tid + "','" + tName + "','" + beginTime + "','" + endTime + "','VOL_DMP','" + beginTime + "');");

            //    sb.Append("INSERT INTO T_FW_TASK_PROC_LOG(TFTL_ID, TFTRL_NAME, TFTRL_ST_DATE, TFTRL_EN_DATE)VALUES ('");
            //    sb.Append(tid + "','P_C_SAVE_SN_TEST','" + beginTime + "','" + endTime + "');");

            //    foreach (string key in htDmp.Keys)
            //    {
            //        string type = "IN";
            //        if ("RES".Equals(key))
            //            type = "OUT";
            //        sb.Append("INSERT INTO T_FW_TASK_PROC_PARA_LOG(TFTL_ID, TFTRL_NAME, TFTPL_NAME, TFTPL_VALUE, TFTPL_TYPE, TFTL_DATE)VALUES ('");
            //        sb.Append(tid + "','P_C_SAVE_SN_TEST','" + key + "','" + htDmp[key] + "','" + type + "',SYSDATE);");
            //    }

            //    sb.Append("END;");
            //    string sa = sb.ToString();
            //    dbHelper.ExecuteSql(sb.ToString());
            //}
            //catch { }

            return ht;
        }
        #endregion

    }
}
