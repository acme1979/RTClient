using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BaseModel;
using System.Drawing.Printing;
using ThoughtWorks.QRCode.Codec;

namespace WorkStation
{
    public partial class LabelPrint : CUserControl, ICommonWorkStationCtl
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
        /// 打印数量
        /// </summary>
        int PrintNum = 0;
        /// <summary>
        /// 制令单计划数量
        /// </summary>
        int TargetNum = 0;
        /// <summary>
        /// SN数量
        /// </summary>
        int SnNum = 0;
        /// <summary>
        /// 最小流水号
        /// </summary>
        int MinNum = 0;
        /// <summary>
        /// 最大流水号
        /// </summary>
        int MaxNum = 0;
        /// <summary>
        /// 起始流水号
        /// </summary>
        int StartNum = 0;
        /// <summary>
        /// 工单号
        /// </summary>
        string ProjectId = "";
        /// <summary>
        /// 制令单号
        /// </summary>
        string MoNumber = "";
        /// <summary>
        /// 产品类型
        /// </summary>
        string ProductType = "";
        /// <summary>
        /// 生产基地
        /// </summary>
        string ProductArea = "";
        /// <summary>
        /// 机种料号
        /// </summary>
        string ModelCode = "";
        /// <summary>
        /// 机种型号
        /// </summary>
        string ItemSpec = "";
        /// <summary>
        /// 打印SN
        /// </summary>
        string PrintSn = "";
        /// <summary>
        /// 打印SN列表
        /// </summary>
        DataTable PrintTable;
        /// <summary>
        /// 纸张宽度
        /// </summary>
        readonly int PageWidth = 120;
        /// <summary>
        /// 纸张高度
        /// </summary>
        readonly int PageHeight = 72;

        public LabelPrint()
        {
            InitializeComponent();
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

        #region 查询按钮点击
        private void button1_Click(object sender, EventArgs e)
        {
            string res = "OK";
            res = CheckData();
            if ("OK".Equals(res))
            {
                dataGridView1.DataSource = PrintTable;
                lblMsg("OK", "OK：查询完成");
                return;
            }
            else
            {
                lblMsg("NG", res);
                return;
            }
        }
        #endregion

        #region 打印按钮点击
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string res = "OK";
            res = CheckData();
            if ("OK".Equals(res))
            {
                for (int i = 0; i < PrintTable.Rows.Count; i++)
                {
                    PrintSn = PrintTable.Rows[i]["工单SN号"].ToString();
                    Print();
                }
                SaveLogs(UsrCode, DATA_AUTH, "打印成功", "\"PRODUCT_AREA\":" + "\"" + ProductArea + "\"" + "," + "\"MO_NUMBER\":" + "\"" + MoNumber + "\"" + "," + "\"START_NUM\":" + "\"" + StartNum.ToString() + "\"" + "," + "\"PRINT_NUM\":" + "\"" + PrintNum.ToString() + "\"", "SN打印模块", "true");
                lblMsg("OK", "OK：打印完成");
            }
            else
            {
                lblMsg("NG", res);
            }
        }
        #endregion

        #region 数据检查
        private string CheckData()
        {
            string res = "OK";
            if ("".Equals(cbbProductType.Text))
            {
                res = "NG：请选择产品类型";
                return res;
            }
            if ("".Equals(cbbProductArea.Text))
            {
                res = "NG：请选择生产基地";
                return res;
            }
            ProductArea = cbbProductArea.Text.Substring(0, cbbProductArea.Text.IndexOf("-"));
            if ("".Equals(tbMoNumber.Text))
            {
                res = "NG：请输入制令单号";
                return res;
            }
            DataTable dt01 = SelectMoBaseByMoNumber(tbMoNumber.Text, data_auth);
            if (dt01.Rows.Count < 1)
            {
                res = "NG：制令单不存在";
                return res;
            }
            ProjectId = dt01.Rows[0]["PM_PROJECT_ID"].ToString();
            MoNumber = dt01.Rows[0]["PM_MO_NUMBER"].ToString();
            ModelCode = dt01.Rows[0]["PM_MODEL_CODE"].ToString();
            ItemSpec = dt01.Rows[0]["CI_ITEM_SPEC"].ToString();
            DataTable dt02 = SelectPmProjectSnByProjectId(ProjectId, data_auth);
            if (dt02.Rows.Count < 1)
            {
                res = "NG：工单SN不存在";
                return res;
            }
            SnNum = dt02.Rows.Count;
            PrintNum = (int)nudPrintNum.Value;
            StartNum = (int)nudStartSn.Value;
            TargetNum = Convert.ToInt16(dt01.Rows[0]["PM_TARGET_QTY"].ToString());
            lbTargetQty.Text = TargetNum.ToString();
            if (PrintNum > SnNum)
            {
                res = "NG：打印数量不能大于工单SN数量";
                return res;
            }
            if (PrintNum > TargetNum)
            {
                res = "NG：打印数量不能大于制令单计划数量";
                return res;
            }
            DataTable dt03 = SelectMaxMinSnByProjectId(ProjectId, data_auth);
            if (dt03.Rows.Count < 1)
            {
                res = "NG：工单SN信息异常";
                return res;
            }
            MaxNum = Convert.ToInt16(dt03.Rows[0]["MAX_NUM"].ToString());
            MinNum = Convert.ToInt16(dt03.Rows[0]["MIN_NUM"].ToString());
            if (StartNum < MinNum || StartNum > MaxNum)
            {
                res = "NG：起始流水号不在工单SN号段内";
                return res;
            }
            if ((StartNum + PrintNum) - 1 > MaxNum)
            {
                res = "NG：打印的结束流水号超出工单SN号段";
                return res;
            }
            PrintTable = SelectPmProjectSnByProjectId1(ProjectId, data_auth, StartNum, PrintNum);
            if (PrintTable.Rows.Count < 1)
            {
                res = "NG：无可打印的工单SN信息";
                return res;
            }
            return res;
        }
        #endregion

        #region 制令单号回车
        private void tbMoNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!("".Equals(tbMoNumber.Text)))
                {
                    DataTable dt01 = SelectMoBaseByMoNumber(tbMoNumber.Text, data_auth);
                    if (dt01.Rows.Count > 0)
                    {
                        lbTargetQty.Text = dt01.Rows[0]["PM_TARGET_QTY"].ToString();
                        DataTable dt02 = SelectMaxMinSnByProjectId(dt01.Rows[0]["PM_PROJECT_ID"].ToString(), data_auth);
                        if (dt02.Rows.Count < 1)
                        {
                            lblMsg("NG", "NG：工单SN信息异常");
                        }
                        lblEndSn.Text = dt02.Rows[0]["MAX_NUM"].ToString();
                        lblStartSn.Text = dt02.Rows[0]["MIN_NUM"].ToString();
                    }
                    else
                    {
                        lbTargetQty.Text = "0";
                        lblMsg("NG", "制令单不存在");
                    }
                }
            }
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

        private DataTable SelectMoBaseByMoNumber(string monumber, string dataauth)
        {
            string sql = string.Format(@"SELECT * FROM T_PM_MO_BASE PMB
                                         LEFT JOIN T_CO_ITEM CI
                                         ON PMB.PM_MODEL_CODE = CI.CI_ITEM_CODE
                                         WHERE PMB.PM_MO_NUMBER = '{0}' AND PMB.DATA_AUTH = '{1}'", monumber, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_PM_MO_BASE");
            return dt;
        }

        private DataTable SelectPmProjectSnByProjectId(string projectid, string dataauth)
        {
            string sql = string.Format(@"SELECT * FROM T_PM_PROJECT_SN PPS
                                         WHERE PPS.PROJECT_ID = '{0}' AND PPS.DATA_AUTH = '{1}' ORDER BY PPS.PRODUCT_SN", projectid, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_PM_MO_BASE");
            return dt;
        }

        private DataTable SelectMaxMinSnByProjectId(string projectid, string dataauth)
        {
            string sql = string.Format(@"SELECT MAX(TO_NUMBER(LTRIM(SUBSTR(PPS.PRODUCT_SN,9),'0'))) MAX_NUM , MIN(TO_NUMBER(LTRIM(SUBSTR(PPS.PRODUCT_SN,9),'0'))) MIN_NUM
                                         FROM T_PM_PROJECT_SN PPS
                                         WHERE PPS.PROJECT_ID = '{0}' AND PPS.DATA_AUTH = '{1}'", projectid, dataauth);
            DataTable dt = dbHelper.GetDataTable(sql, "T_PM_PROJECT_SN");
            return dt;
        }

        private DataTable SelectPmProjectSnByProjectId1(string projectid, string dataauth, int startnum, int printnum)
        {
            string sql = string.Format(@"SELECT * FROM (SELECT PMB.PM_MO_NUMBER 制令单号 , PMB.PM_MODEL_CODE 机种料号 , CI.CI_ITEM_SPEC 机种规格 , PPS.PRODUCT_SN 工单SN号 , PPS.CREATE_TIME 创建时间 FROM T_PM_PROJECT_SN PPS
                                         RIGHT JOIN T_PM_MO_BASE PMB
                                         ON PMB.PM_PROJECT_ID = PPS.PROJECT_ID
                                         LEFT JOIN T_CO_ITEM CI
                                         ON CI.CI_ITEM_CODE = PMB.PM_MODEL_CODE
                                         WHERE PPS.PROJECT_ID = '{0}'
                                         AND PPS.DATA_AUTH = '{1}'
                                         AND TO_NUMBER(LTRIM(SUBSTR(PPS.PRODUCT_SN,9),'0')) >= {2}
                                         ORDER BY PPS.PRODUCT_SN)
                                         WHERE ROWNUM <= {3}", projectid, dataauth, startnum, printnum);
            DataTable dt = dbHelper.GetDataTable(sql, "T_PM_MO_BASE");
            return dt;
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

        #region Print
        public void Print()
        {
            try
            {
                PrintDocument printdocument1 = new PrintDocument();

                PrintController printController = new StandardPrintController();

                printdocument1.PrintController = printController;

                printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", PageWidth, PageHeight);

                //printDocument1.DefaultPageSettings.Landscape = true;

                printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

                printDocument1.Print();

                printDocument1.PrintPage -= new PrintPageEventHandler(printDocument1_PrintPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            DrawPrint1(e);
        }

        public void DrawPrint1(PrintPageEventArgs e)
        {
            try
            {
                //e.Graphics.DrawString("武汉恒发", new System.Drawing.Font(new FontFamily("微软雅黑"), 14, FontStyle.Regular), System.Drawing.Brushes.Black, 10, 20);

                //Pen blackPen = new Pen(Color.Black,1);
                //e.Graphics.DrawLine(blackPen, 0, 0, 120, 0);
                //e.Graphics.DrawLine(blackPen, 0, 72, 120, 72);
                //e.Graphics.DrawLine(blackPen, 0, 0, 0, 72);
                //e.Graphics.DrawLine(blackPen, 120, 0, 120, 72);

                e.Graphics.DrawImage(CreateQrcodePicture(PrintSn), 8, 8, 28, 28);

                e.Graphics.DrawImage(CreateQrcodePicture(PrintSn), 84, 8, 28, 28);

                e.Graphics.DrawString(ProductArea, new Font(new FontFamily("微软雅黑"), 8, FontStyle.Regular), System.Drawing.Brushes.Black, 48, 12);

                e.Graphics.DrawString(PrintSn, new Font(new FontFamily("微软雅黑"), 6, FontStyle.Regular), System.Drawing.Brushes.Black, 6, 36);
                e.Graphics.DrawString(ModelCode, new Font(new FontFamily("微软雅黑"), 6, FontStyle.Regular), System.Drawing.Brushes.Black, 6, 44);
                e.Graphics.DrawString(ItemSpec, new Font(new FontFamily("微软雅黑"), 6, FontStyle.Regular), System.Drawing.Brushes.Black, 6, 52);
                e.Graphics.DrawString(MoNumber, new Font(new FontFamily("微软雅黑"), 6, FontStyle.Regular), System.Drawing.Brushes.Black, 6, 60);

                //e.Graphics.DrawImage(CreateQrcodePicture(qrCode1), 15, 70, 230, 230);

                //e.Graphics.DrawImage(CreateSpireBarcodePicture(qrCode1), 250, 10, 270, 450);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public System.Drawing.Image CreateQrcodePicture(string qrCode)
        {
            //创建二维码生成类  
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度  
            qrCodeEncoder.QRCodeScale = 2;
            //设置编码版本  
            qrCodeEncoder.QRCodeVersion = 0;
            //设置编码错误纠正  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //qrCodeEncoder.Encode(qrCode, Encoding.UTF8);
            //生成二维码图片  
            System.Drawing.Image image = qrCodeEncoder.Encode(qrCode, Encoding.UTF8);
            return image;
        }

        #endregion

        #region 提示音结束
        private void timerMusic_Tick_1(object sender, EventArgs e)
        {
            sound.passPlayer.Stop();
            sound.sndplayer.Stop();
            timerMusic.Enabled = false;
        }
        #endregion
    }
}
