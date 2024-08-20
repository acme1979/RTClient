using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
//using AutoMachineDataRead;
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

        //已加载的流程控制控件
        ICommonWorkStationCtl iCWSCtl = null;

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
            btnYC.Click += new EventHandler(btnYC_Click);
        }

        #endregion

        #region btnYC_Click
        private void btnYC_Click(object sender, EventArgs e)
        {

            int pw = panel7.Width;
            if (pw > 30)
            {
                panel7.Width = 30;
                btnYC.Text = "显\r\n\r\n示";
            }
            else
            {
                Rectangle ScreenArea = System.Windows.Forms.Screen.GetBounds(this);
                int width1 = ScreenArea.Width; //屏幕宽度
                panel7.Width = Convert.ToInt32(ScreenArea.Width * 0.15);
                btnYC.Text = "隐\r\n\r\n藏";
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
                LoadWorkStationCtl(ParamSetting.Process);
            }
            catch (Exception ex)
            {
                MessageBox.Show("未知异常：" + ex.Message);
            }
        }
        #endregion


        #region LoadWorkStationCtl加载窗体
        private void LoadWorkStationCtl(string Process)
        {
            panel1.Visible = false;
            Rectangle ScreenArea = System.Windows.Forms.Screen.GetBounds(this);
            int width1 = ScreenArea.Width; //屏幕宽度
            panel7.Width = Convert.ToInt32(ScreenArea.Width * 0.15);


            string groupCode = workStation(Process);
            //groupCode = "PackInput1111111";

            AppSettingsReader appReader = new AppSettingsReader();
            string IsHeartBeat = appReader.GetValue("IsHeartBeat", typeof(string)).ToString();
            string HeartBeatTime = appReader.GetValue("HeartBeatTime", typeof(string)).ToString();
            int HBT = 800000;
            int.TryParse(HeartBeatTime, out HBT);

            #region 菱电-炉温文件上传
            if (groupCode == "FileUpload-LW")
            {
                UpLoadFile_LW icws = new UpLoadFile_LW();
                pnlExtend.Height = icws.Height;
                icws.Name = "FileUploadLWCtl";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-ECU自动线
            else if (groupCode == "VoluntarilyECU")
            {
                TimerReadECU icws = new TimerReadECU();
                pnlExtend.Height = icws.Height;
                icws.Name = "voluntarilyECU";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-SMT-YS  钢网检查
            else if (groupCode == "CheckStencil")
            {
                TimerReadStencil icws = new TimerReadStencil();
                pnlExtend.Height = icws.Height;
                icws.Name = "timerReadStencil";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-SMT-镭雕  镭雕PCB多板关联
            else if (groupCode == "LaserEngravingMachine")
            {
                TaskLaserEngravingMachine icws = new TaskLaserEngravingMachine();
                pnlExtend.Height = icws.Height;
                icws.Name = "taskLaserEngravingMachine";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-SMT-SPI
            else if (groupCode == "SMT-SPI")
            {
                TimerReadSPI icws = new TimerReadSPI();
                pnlExtend.Height = icws.Height;
                icws.Name = "timerReadSPI";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-SMT-AOI
            else if (groupCode == "SMT-AOI")
            {
                TimerReadAOI icws = new TimerReadAOI();
                pnlExtend.Height = icws.Height;
                icws.Name = "timerReadAOI";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-DIP-FCT
            else if (groupCode == "DIP-FCT")
            {
                TimerReadFCT icws = new TimerReadFCT();
                pnlExtend.Height = icws.Height;
                icws.Name = "timerReadFCT";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-DIP-老化及台架测试
            else if (groupCode == "DIP_Burn_in")
            {
                TaskBurnIn icws = new TaskBurnIn();
                pnlExtend.Height = icws.Height;
                icws.Name = "taskBurnIn";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-DIP-ICT
            else if (groupCode == "DIP-ICT")
            {
                TaskICT icws = new TaskICT();
                pnlExtend.Height = icws.Height;
                icws.Name = "taskICT";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-X-Ray文件上传
            else if (groupCode == "X-Ray")
            {
                TaskX_Ray icws = new TaskX_Ray();
                pnlExtend.Height = icws.Height;
                icws.Name = "taskX_Ray";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-ECU贴标
            else if (groupCode == "ECU-TB")
            {
                EcuLabel icws = new EcuLabel();
                pnlExtend.Height = icws.Height;
                icws.Name = "ecuLabel";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-程序文件下载
            else if (groupCode == "FileDownload")
            {
                FileDownload icws = new FileDownload();
                pnlExtend.Height = icws.Height;
                icws.Name = "filedownload";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-标签生成打印
            else if (groupCode == "LabelPrint")
            {
                LabelPrint icws = new LabelPrint();
                pnlExtend.Height = icws.Height;
                icws.Name = "labelprint";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-产品信息查询
            else if (groupCode == "SN-QUERY")
            {
                SnQuery icws = new SnQuery();
                pnlExtend.Height = icws.Height;
                icws.Name = "SnQuery";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion
            #region 菱电-包装模块
            else if (groupCode == "PACKING")
            {
                Packing icws = new Packing();
                pnlExtend.Height = icws.Height;
                icws.Name = "Packing";
                icws.Dock = DockStyle.Fill;
                icws.Active();
                iCWSCtl = icws;
                iCWSCtl.DATA_AUTH = ParamSetting.DATA_AUTH;
                iCWSCtl.DBCode = DBCode;
                iCWSCtl.iDBHelper = dbHelper;
                pnlExtend.Controls.Add(icws);
            }
            #endregion

            #region 默认参数界面
            else
            {
                ScanMultiProductWSCtl icws = new ScanMultiProductWSCtl();
                pnlExtend.Height = icws.Height;
                icws.Name = "scanMultiProductWSCtl";
                icws.Dock = DockStyle.Fill;
                icws.ParentCtl = this;
                icws.Active();
                pnlExtend.Controls.Add(icws);
                iCWSCtl = icws;
            }
            #endregion
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

            iCWSCtl.InitData();
        }
        #endregion

        #region btnDB_Click
        private void btnDB_Click(object sender, EventArgs e)
        {
            DBSetupFrm db = new DBSetupFrm();
            db.Show();
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