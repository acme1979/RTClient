using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AutoMachineDataRead;
using BaseModel;
//using RTClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;


namespace WorkStation
{
    public partial class WorkStationCtl : CUserControl
    {
        #region 构造函数及成员
        public CParamSetting ParamSetting { get; set; }
        /// <summary>
        /// CMDCODE 指令
        /// </summary>
        private enum ScanBarState { USER, INIT, WORK, CMDCODE };
        private ScanBarState m_ScanBarState = ScanBarState.WORK;

        /// <summary>
        /// 是否权限校验
        /// </summary>
        private bool isEmpRole = true;
        /// <summary>
        /// 指令ID
        /// </summary>
        private string M_CUR_CMD_CODE = "";

        //已加载的流程控制控件
        ICommonWorkStationCtl iCWSCtl = null;
        private bool clearWinFlag = false;
        //用于记录当前工序ID
        private int ProcessId = 0;
        /// <summary>
        ///声音控制
        /// </summary>
        SoundPlayerHelp sound = new SoundPlayerHelp();

        /// <summary>
        /// 控件声音响起时长
        /// </summary>
        int IntI = 0;

        string DATA_AUTH = "";
        /// <summary>
        /// 数据库类型
        /// </summary>
        string DBCode = "";
        /// <summary>
        /// 数据库链接
        /// </summary>
        IDBHelper dbHelper = null;
        public WorkStationCtl()
        {
            InitializeComponent();
            InitCtl();
        }
        #endregion

        #region InitCtl()
        private void InitCtl()
        {
            StyleHelper.Instance.SetStyle(btnSetup);
            StyleHelper.Instance.SetStyle(btnDB);
            this.Load += new EventHandler(WorkStationCtl_Load);
            btnSetup.Click += new EventHandler(btnSetup_Click);
            btnDB.Click += new EventHandler(btnDB_Click);
            this.timer1.Tick += new EventHandler(timer1_Tick);
            this.timerMusic.Tick += new EventHandler(timerMusic_Tick);
            this.tbScanData.KeyDown += new KeyEventHandler(txtScanData_KeyDown);
            this.dataGridView2.MouseDown += new MouseEventHandler(dataGridView2_MouseDown);
            InitSubCtl();
        }
        #endregion

        #region 扫描回车事件
        /// <summary>
        /// 扫描回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtScanData_KeyDown(object sender, KeyEventArgs e)
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

        #region btnSetup_Click
        private void btnSetup_Click(object sender, EventArgs e)
        {
            SetupFrm frm = new SetupFrm();
            frm.ParamSetting = this.ParamSetting;
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                this.Dispose();
                GC.Collect();
                Application.Exit();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.FileName = "RTCLient.exe";
                System.Diagnostics.Process.Start(startInfo);
            }
        }
        #endregion

        #region WorkStationCtl_Load
        private void WorkStationCtl_Load(object sender, EventArgs e)
        {
            try
            {

                string fileName = Directory.GetCurrentDirectory() + "\\Config\\WorkStationParamSetting.xml";
                if (!File.Exists(fileName))
                {
                    SetupFrm frm = new SetupFrm();
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.ParamSetting = frm.ParamSetting;
                    }
                }
                else
                {
                    CParamSetting ips = new CParamSetting();
                    ips.Deserializer();
                    ParamSetting = ips;
                }
                tbPName.Tag = ParamSetting.Process;
                tbPName.Text = ParamSetting.ProcessName;
                tbWSName.Tag = ParamSetting.WorkStation;
                tbWSName.Text = ParamSetting.WorkStationName;
                tbProductLine.Text = ParamSetting.ProductLineName;
                tbProductLine.Tag = ParamSetting.ProductLine;
                tbUId.Text = ParamSetting.EmpCode;
                tbUName.Text = ParamSetting.EmpName;
                DBCode = ParamSetting.DBCode;
                dbHelper = DBHelper.Instance.GetDBInstace(DBCode);
                //加载制令单信息
                ScanPO("");
                LoadWorkStationCtl(ParamSetting.Process);
                focus();
                //lblRltState.Text = "NG";
                //lblRltState.BackColor = Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show("未知异常：" + ex.Message);
            }
        }
        #endregion

        #region KeyDownEventMethod 无法区分大小写不在使用
        public void KeyDownEventMethod(object sender, KeyEventArgs e)
        {
            try
            {
                string keyCode = "";
                if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
                {
                    keyCode = ((char)e.KeyCode).ToString();
                }
                else if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                {
                    if (e.Shift)
                    {
                        keyCode = ")!@#$%^&*(".Substring(e.KeyCode - Keys.D0, 1);
                    }
                    else
                    {
                        keyCode = ((char)e.KeyCode).ToString();
                    }
                }
                else if ((int)e.KeyCode == 189 || (int)e.KeyCode == 191 || (int)e.KeyCode == 190 || (int)e.KeyCode == 166 || (int)e.KeyCode == 32)
                {
                    if (e.Shift)
                    {
                        switch ((int)e.KeyCode)
                        {
                            case 189:
                                keyCode = "_";
                                break;
                            case 191:
                                keyCode = "?";
                                break;
                            case 190:
                                keyCode = ">";
                                break;
                            case 166:
                                keyCode = "|";
                                break;
                        }
                    }
                    else
                    {
                        switch ((int)e.KeyCode)
                        {
                            case 189:
                                keyCode = "-";
                                break;
                            case 191:
                                keyCode = "/";
                                break;
                            case 190:
                                keyCode = ".";
                                break;
                        }
                    }
                    if ((int)e.KeyCode == 32)
                    {
                        keyCode = " ";
                    }
                }
                else if ((int)e.KeyCode == 124)
                {
                    keyCode = ((char)e.KeyCode).ToString();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    ScanDataOperate(lblScanData.Text);
                    lblScanData.Tag = "Return";
                    return;
                }
                if (lblScanData.Tag != null && lblScanData.Tag.ToString() == "Return")
                {
                    lblScanData.Text = "";
                    lblScanData.Tag = keyCode;
                }
                else
                {
                    lblScanData.Tag = keyCode;
                }
                lblScanData.Text = lblScanData.Text + keyCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("键入内容异常:" + ex.Message);
            }
        }
        #endregion

        #region ScanDataOperate(string barString)
        private void ScanDataOperate(string barString)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                barString = barString.Trim();
                //if (barString == "%CLS%")
                //{
                //    InitSubCtl();
                //    IntI = 1;
                //    WriteScanHint("CLEAR", "错误清除完成...", true, true);
                //    return;
                //}
                //if (barString == "%CLSDATA%")
                //{
                //    InitDataCtl();
                //    return;
                //}
                //if (IntI == 1)
                //{
                //    InitDataCtl();
                //    WriteScanHint(barString, "请先清除错误信息!", false, false);
                //    return;
                //}
                if (m_ScanBarState == ScanBarState.USER)
                {
                    ScanUID(barString);
                    return;
                }
                if (m_ScanBarState == ScanBarState.CMDCODE)
                {

                    return;
                }
                if (m_ScanBarState == ScanBarState.WORK)
                {
                    ScanProductBarcode(barString);
                    return;
                }
                if (m_ScanBarState == ScanBarState.INIT)
                {

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现未知异常：" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region InitSubCtl()
        private void InitSubCtl()
        {
            m_ScanBarState = ScanBarState.WORK;
            lblScanHint.Text = "请扫描员工号";
            lblScanHint.Tag = "请扫描员工号";
            tbUId.Text = "";
            tbUName.Text = "";
            lvPO.Items.Clear();
            if (iCWSCtl != null)
            {
                iCWSCtl.InitData();
                iCWSCtl.BadList.Clear();
            }
        }
        #endregion

        #region InitDataCtl()
        private void InitDataCtl()
        {
            if (lvPO.Items.Count <= 0)
            {
                lblScanHint.Text = "请扫描待生产工单";
                lblScanHint.Tag = "请扫描待生产工单";
            }
            else
            {
                lblScanHint.Text = "请扫描产品条码";
                lblScanHint.Tag = "请扫描产品条码";
            }

            IntI = 1;
            //WriteScanHint("CLEAR", "错误清除完成...", true, true);
            if (iCWSCtl != null)
            {
                iCWSCtl.InitData();
                iCWSCtl.BadList.Clear();
            }
        }
        #endregion

        #region 扫描用户ID执行过程
        private void ScanUID(string barString)
        {
            //校验员工信息
            List<DBParameters> ListDBParameters = new List<DBParameters>();
            dbHelper.AddDBParameters(ListDBParameters, "DATA_AUTH", ParamSetting.DATA_AUTH, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "M_EMP", barString, ParameterDirection.Input, valueTypes.STRING);
            dbHelper.AddDBParameters(ListDBParameters, "RES", null, ParameterDirection.Output, valueTypes.STRING);

            Hashtable htUser = dbHelper.htExecuteNonQuery(ListDBParameters, "P_C_CHECK_EMP");//存储中增加名称参数
            if (!htUser["RES"].ToString().StartsWith("OK"))
            {
                WriteScanHint(barString, htUser["RES"].ToString(), "NG");
                return;
            }
            string res = htUser["RES"].ToString().Split(':')[0].ToString();
            string name = htUser["RES"].ToString().Split(':')[1].ToString();
            tbUId.Text = barString;
            tbUName.Text = name;
            iCWSCtl.UsrCode = barString;
            iCWSCtl.UsrName = name;
            WriteScanHint(barString, "人员信息读取正确...", "OK");
            if (tbPName.Tag.ToString() == "CastMaterial")
            {
                m_ScanBarState = ScanBarState.WORK;
            }
            else
            {
                m_ScanBarState = ScanBarState.CMDCODE;
            }

            //if (isEmpRole)
            //{
            //    str = new string[4];
            //    str[0] = "INPUT,STRING,DATA_AUTH," + ParamSetting.DATA_AUTH;
            //    str[1] = "INPUT,STRING,M_EMP,lk";
            //    str[2] = "INPUT,STRING,M_CUR_CMD_CODE," + M_CUR_CMD_CODE;
            //    str[3] = "OUTPUT,STRING,RES,";
            //    htUser = OracleDBHelper.htExecuteNonQuery(str, "P_C_CHECK_EMPROLE");
            //}

        }
        #endregion

        #region 扫描制令单执行过程
        private void ScanPO(string barString)
        {
            string sql = "SELECT T.PMO_NUMBER,T.PMO_PROJECT_ID,T.PMO_TYPE,T.PMO_AREA_SN,T.PMO_MODEL_CODE,T2.CI_ITEM_NAME,T2.CI_ITEM_SPEC,T1.PM_TARGET_QTY " +
                         " FROM T_PM_MO_ONLINE T " +
                         " LEFT JOIN T_PM_MO_BASE T1 ON T.PMO_NUMBER = T1.PM_MO_NUMBER AND T.DATA_AUTH = T1.DATA_AUTH " +
                         " LEFT JOIN T_CO_ITEM T2 ON T.PMO_MODEL_CODE = T2.CI_ITEM_CODE AND T.DATA_AUTH = T2.DATA_AUTH " +
                         "WHERE T.PMO_AREA_SN = '" + tbProductLine.Tag + "' " +
                         "  AND T.PMO_TYPE = '1' " +
                         "  AND T.DATA_AUTH = '" + ParamSetting.DATA_AUTH + "' ";
            DataTable dtPMO = dbHelper.GetDataTable(sql, "T_PM_MO_ONLINE");
            if (dtPMO.Rows.Count != 1)
            {
                return;
            }

            #region 加载制令单信息
            lvPO.Items.Clear();
            ListViewItem lvi = new ListViewItem("制令单号");
            lvi.SubItems.Add(dtPMO.Rows[0]["PMO_NUMBER"].ToString());
            lvPO.Items.Add(lvi);

            lvi = new ListViewItem("品号");
            lvi.SubItems.Add(dtPMO.Rows[0]["PMO_MODEL_CODE"].ToString());
            lvPO.Items.Add(lvi);

            lvi = new ListViewItem("品名");
            lvi.SubItems.Add(dtPMO.Rows[0]["CI_ITEM_NAME"].ToString());
            lvPO.Items.Add(lvi);

            lvi = new ListViewItem("规格");
            lvi.SubItems.Add(dtPMO.Rows[0]["CI_ITEM_SPEC"].ToString());
            lvPO.Items.Add(lvi);

            lvi = new ListViewItem("计划数量");
            lvi.SubItems.Add(dtPMO.Rows[0]["PM_TARGET_QTY"].ToString());
            lvPO.Items.Add(lvi);
            lvPO.Tag = barString;


            #endregion

            #region 完成后处理
            //iCWSCtl.PO = barString;
            //iCWSCtl.InitData();
            //m_ScanBarState = ScanBarState.WORK;
            //WriteScanHint(barString, "工单读取正确...", true, true);
            //lblScanHint.Text = "请扫描产品条码";
            //lblScanHint.Tag = "请扫描产品条码";
            #endregion

            clearWinFlag = true;
        }
        #endregion

        #region timer1_Tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            string sTmp = "";
            sTmp = lblScanHint.Text.Replace(lblScanHint.Tag.ToString(), "");
            sTmp = sTmp.Replace("-", "");
            if (sTmp.Length <= 4)
            {
                sTmp = sTmp + ">";
                lblScanHint.Text = lblScanHint.Tag.ToString() + "-" + sTmp;
            }
            else
            {
                sTmp = ">";
                lblScanHint.Text = lblScanHint.Tag.ToString() + "-" + sTmp;
            }
        }
        #endregion

        #region WriteScanHint变更扫描记录
        /// <summary>
        /// 变更扫描记录
        /// </summary>
        /// <param name="barString">产品SN</param>
        /// <param name="sHint">提示信息</param>
        /// <param name="resStart">状态</param>
        private void WriteScanHint(string barString, string sHint, string resStart)
        {
            dataGridView2.Rows.Insert(0, 1);
            DataGridViewRow row = dataGridView2.Rows[0];
            row.Cells["SN"].Value = dataGridView2.Rows.Count;
            row.Cells["DC01"].Value = barString;
            if (resStart == "OK")
            {
                row.Cells["DC02"].Value = "OK";
                lblRltState.Text = "OK";
                lblRltState.BackColor = Color.Green;
                IntI = 0;
                sound.passPlayer.PlayLooping();//播放通过提示音
                timerMusic.Enabled = true;
            }
            else if (resStart == "NG")
            {
                row.Cells["DC02"].Value = "NG";
                lblRltState.Text = "NG";
                lblRltState.BackColor = Color.Red;
                IntI = 0;
                sound.sndplayer.PlayLooping();//播放通过提示音
                timerMusic.Interval = 2000;
                timerMusic.Enabled = true;
            }
            else
            {
                row.Cells["DC02"].Value = "ERROR";
                lblRltState.Text = "ERROR";
                lblRltState.BackColor = Color.Yellow;
                IntI = 1;
                sound.sndplayer.PlayLooping();//播放通过提示音
                timerMusic.Interval = 2000;
                timerMusic.Enabled = true;
            }
            row.Cells["DC03"].Value = sHint;

            if (iCWSCtl.HtCommonParam.ContainsKey("BadProduct"))
            {
                iCWSCtl.HtCommonParam.Remove("BadProduct");
            }
        }
        #endregion

        #region 扫描产品条码
        public void ScanProductBarcode(string barString)
        {

            CScanResult sr = iCWSCtl.ExecScanBarOperate(barString);

            lblScanHint.Text = "请扫描产品条码";
            lblScanHint.Tag = "请扫描产品条码";
            if (sr.Result != "OK")
            {
                WriteScanHint(sr.BarString, sr.Remark, "NG");
            }
            else
            {
                WriteScanHint(sr.BarString, sr.Remark, "OK");
            }

        }
        #endregion

        #region LoadWorkStationCtl加载窗体
        private void LoadWorkStationCtl(string Process)
        {
            string groupCode = workStation(Process);

            AppSettingsReader appReader = new AppSettingsReader();
            string IsHeartBeat = appReader.GetValue("IsHeartBeat", typeof(string)).ToString();
            string HeartBeatTime = appReader.GetValue("HeartBeatTime", typeof(string)).ToString();
            int HBT = 800000;
            int.TryParse(HeartBeatTime, out HBT);

            #region 分选工站
            #region PCB板标签打印
            if (groupCode == "PrintLabelPCB")
            {
                PrintLabelPCBCtl icws = new PrintLabelPCBCtl();
                pnlExtend.Height = icws.Height;
                pnlExtend.Dock = DockStyle.Fill;
                icws.Name = "PrintLabelPCBCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                icws.InitData();
                icws.ParamSetting = ParamSetting;
                iCWSCtl = icws;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
                isEmpRole = false;
                pnlScan.Visible = false;
                pnlRltState.Visible = false;
                pnlCheckRlt.Visible = false;

            }
            #endregion
            else if (groupCode == "CastMaterial")
            {
                CastMaterialWSCtl icws = new CastMaterialWSCtl();
                pnlExtend.Height = icws.Height;
                pnlExtend.Dock = DockStyle.Top;
                icws.Name = "castMaterialWSCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                pnlExtend.Controls.Add(icws);
                iCWSCtl = icws;
                isEmpRole = false;
            }
            #endregion
            #region 生产组装(组焊)
            else if (groupCode == "ProductionAssemble")
            {
                ProductionAssembleWSCtl icws = new ProductionAssembleWSCtl();
                pnlExtend.Height = icws.Height;
                pnlExtend.Dock = DockStyle.Top;
                icws.Name = "productionAssembleWSCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                icws.InitData();
                pnlExtend.Controls.Add(icws);
                iCWSCtl = icws;
                isEmpRole = false;
            }
            #endregion
            #region 生产测试指令
            else if (groupCode == "TimerRead")
            {
                TimerReadWSCtl icws = new TimerReadWSCtl();
                pnlExtend.Height = icws.Height + pnlRltState.Height;
                pnlExtend.Dock = DockStyle.Top;
                icws.Name = "timerReadWSCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                icws.InitData();
                pnlExtend.Controls.Add(icws);
                iCWSCtl = icws;
                isEmpRole = false;
                pnlScan.Visible = false;
                pnlCheckRlt.Visible = false;
                pnlRltState.Visible = false;
            }
            #endregion
            #region 包装-信申
            else if (groupCode == "Packing")
            {
                PackingWSCtl icws = new PackingWSCtl();
                pnlExtend.Height = icws.Height;
                pnlExtend.Dock = DockStyle.Top;
                icws.Name = "packingWSCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                //icws.InitData();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                if ("TRUE".Equals(IsHeartBeat.ToUpper()))
                {
                    iCWSCtl.IsHeartBeat = true;
                    iCWSCtl.HeartBeatTime = HBT;
                }
                else
                    iCWSCtl.IsHeartBeat = false;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 定时读取File文件夹，解析数据-性能测试
            else if (groupCode == "timerReadFile")
            {
                TimerReadFileWSCtl icws = new TimerReadFileWSCtl();
                pnlExtend.Height = icws.Height + pnlRltState.Height + 125;
                pnlExtend.Dock = DockStyle.Top;
                icws.Name = "timerReadAccessWSCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                iCWSCtl = icws;
                if ("TRUE".Equals(IsHeartBeat.ToUpper()))
                {
                    iCWSCtl.IsHeartBeat = true;
                    iCWSCtl.HeartBeatTime = HBT;
                }
                else
                    iCWSCtl.IsHeartBeat = false;
                pnlExtend.Controls.Add(icws);

                isEmpRole = false;
                pnlScan.Visible = false;
                pnlCheckRlt.Visible = false;
                pnlRltState.Visible = false;
            }
            #endregion
            else
            {
                ScanMultiProductWSCtl icws = new ScanMultiProductWSCtl();
                pnlExtend.Height = icws.Height;
                pnlExtend.Dock = DockStyle.Top;
                icws.Name = "scanMultiProductWSCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                pnlExtend.Controls.Add(icws);
                iCWSCtl = icws;
            }
            iCWSCtl.UsrCode = tbUId.Text.Trim();
            iCWSCtl.UsrName = tbUName.Text.Trim();
            iCWSCtl.Process = ParamSetting.Process;
            iCWSCtl.ProcessName = ParamSetting.ProcessName;
            iCWSCtl.ProductLine = ParamSetting.ProductLine;
            iCWSCtl.ProductLineName = ParamSetting.ProductLineName;
            iCWSCtl.WorkStation = ParamSetting.WorkStation;
            iCWSCtl.WorkStationName = ParamSetting.WorkStationName;
            iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
            iCWSCtl.DBCode = DBCode;
            iCWSCtl.iDBHelper = dbHelper;
            if (lvPO.Items.Count > 0 )
                iCWSCtl.PO = lvPO.Items[0].SubItems[1].Text;

            //iCWSCtl.ParentCtl = this;
        }
        #endregion

        #region timerMusic_Tick
        private void timerMusic_Tick(object sender, EventArgs e)
        {
            //IntI += 1;
            //if (IntI == 0 && lblRltState.Text != "ERROR")
            //{
            sound.passPlayer.Stop();
            sound.sndplayer.Stop();
            timerMusic.Enabled = false;
            //}
        }
        #endregion

        #region 更新数据图表
        private void UpdateChart()
        {
            ////Series  对象表示数据系列，并且存储在 SeriesCollection 类中。  
            //Series s1 = this.chartControl1.Series[0];//新建一个series类并给控件赋值  
            //s1.DataSource = GetClassCount();//设置实例对象s1的数据源  
            //s1.ArgumentDataMember = "THOUR";//绑定图表的横坐标  
            //s1.ValueDataMembers[0] = "NUM"; //绑定图表的纵坐标坐标  
            //s1.LegendText = "生产数量";//设置图例文字 就是右上方的小框框  
            //s1.SeriesPointsSorting = SortingMode.None;
            ////s1.SeriesPointsSortingKey = SeriesPointKey.Argument;
            //DevExpress.XtraCharts.StackedBarSeriesView stackedBarSeriesView1 = new DevExpress.XtraCharts.StackedBarSeriesView();
            //this.chartControl1.Series[0].View = stackedBarSeriesView1;
        }

        public DataTable GetClassCount()
        {
            DataTable dt = new DataTable();
            string sql = string.Format("SELECT CONVERT(NVARCHAR(10),T1.THOUR) + '点' THOUR,ISNULL(T2.NUM,0)NUM FROM (SELECT 8 THOUR UNION SELECT 9 UNION SELECT 10 UNION SELECT 11 UNION SELECT 13" +
                "UNION SELECT 14 UNION SELECT 15 UNION SELECT 16 UNION SELECT 17)T1 LEFT JOIN (SELECT DATEPART(HH,CRTDATE) THOUR,COUNT(1)NUM FROM PRCBA " +
                "WHERE CONVERT(NVARCHAR(10),CRTDATE,112) = CONVERT(NVARCHAR(10),GETDATE(),112) AND ISNULL(BA48,0) = 0 AND ISNULL(BA28,'') = '{0}' " +
                "GROUP BY DATEPART(HH,CRTDATE))T2 ON T2.THOUR = T1.THOUR ORDER BY CONVERT(int,T1.THOUR)", tbWSName.Tag.ToString());
            return null;// m_CBiz.ExecQuerySQL(sql, "PRCBA");
        }
        #endregion

        #region btnDB_Click
        private void btnDB_Click(object sender, EventArgs e)
        {
            DBSetupFrm db = new DBSetupFrm();
            db.Show();
        }
        #endregion

        #region focus()
        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            focus();
        }

        private void focus()
        {
            this.tbScanData.Focus();
        }
        #endregion

        #region 获取工站类型
        private string workStation(string Process)
        {
            string sql = "SELECT GROUP_WORKSTATION FROM T_CO_GROUP WHERE GROUP_CODE = '" +
                Process + "' AND DATA_AUTH = '" + ParamSetting.DATA_AUTH + "' ";
            DataTable gWorkStation = dbHelper.GetDataTable(sql, "T_CO_GROUP");
            if (gWorkStation.Rows.Count != 1)
            {
                return "";
            }
            return gWorkStation.Rows[0]["GROUP_WORKSTATION"].ToString();
        }
        #endregion

        #region Closing()母窗体关闭，触发子控件关闭事件
        public override void Closing()
        {
            foreach (CUserControl uc in pnlExtend.Controls)
            {
                uc.Closing();
            }
            base.Closing();
        }
        #endregion
    }
}