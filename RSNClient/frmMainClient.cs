using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BaseModel;

namespace RTClient
{
    public partial class frmMainClient : Form, IGotoForm, ICloseTabPage
    {
        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);


        #region Properties & Members
        //存取当前系统类型
        private string m_systemType = "";

        private CUserControl m_UserControl = null;
        #endregion

        #region TagPages 保存tabpage的激活顺序，最后激活的放在链表右边
        private ArrayList m_TagPagesActive = null;
        public ArrayList TagPagesActive
        {
            get
            {
                if (m_TagPagesActive == null)
                    m_TagPagesActive = new ArrayList();
                return m_TagPagesActive;
            }
            set { m_TagPagesActive = value; }
        }
        #endregion

        #region GotoForm 从当前用户组件切换到另一个用户组件
        /// <summary>
        /// 从当前用户组件切换到另一个用户组件
        /// </summary>
        /// <param name="Id">唯一识别号</param>
        /// <param name="strParam">字符串参数,参数格式为：“ParamName1=ParamValue1;............ParamNameN=ParamValueN”</param>
        /// <param name="dsParam">数据集参数</param>
        /// <returns>返回CForm或CUserControl对象</returns>
        public object GotoForm(string Id, string strParam, DataSet dsParam)
        {
            return null;
        }
        #endregion

        #region 构造函数
        public frmMainClient(string systemType)
        {
            InitializeComponent();
            m_systemType = systemType;
            this.InitCtl();
        }
        #endregion

        #region InitCtl 初始化设置
        /// <summary>
        /// 初始化设置
        /// </summary>
        private void InitCtl()
        {
            this.Load += new EventHandler(frmMainClient_Load);
            this.FormClosing += new FormClosingEventHandler(frmMainClient_FormClosing);
            
        }
        #endregion

        #region frmMainClient_Load 
        /// <summary>
        /// 客户端加载说明，读取配置文件信息《Client》，《ClientType》和《ShowParam》
        /// 《Client》如果它为true时，则显示客户端界面，否则显示登陆界面
        /// 系统以SysteType、《ClientType》和《ShowParam》读取表SYS_SystemClient的信息，字段funcHref的值即为显示窗体控件
        /// 
        /// SYS_SystemClient该表包含字段：SystemType为系统类型，ClientType客户端类型（对应配置文件《ClientType》），
        /// ShowParam显示参数，对应对应配置文件《ShowParam》，funcHref显示控件名，FuncDesc功能描述，sParam系统参数（格式：ParamName1=ParamValue1;............ParamNameN=ParamValueN）;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMainClient_Load(object sender, EventArgs e)
        {
            try
            {
                //if (ConfigurationManager.AppSettings["ShowParam"].ToString() == "LittleForm")
                //{
                //    this.TopMost = false;
                //    this.Width = 412;
                //    this.Height = 280;
                //    this.WindowState = FormWindowState.Normal;
                //}
                object obj = CObjectFactory.Instance.CreateObject("WorkStation.WorkStationCtl","DLL");
                CUserControl uc = (CUserControl)obj;
                uc.Dock = DockStyle.Fill;
                uc.Tag = "Id";
                uc.SystemType = this.m_systemType;
                uc.ControlDesc = "WorkStation";
                uc.BackColor = this.DefaultBgColor;
                //字符串参数,参数格式为：“ParamName1=ParamValue1;............ParamNameN=ParamValueN”
                uc.SetParam(null, null);
                this.Controls.Add(uc);
                m_UserControl = uc;
                //DataTable dt = new DataTable("Templet");
                //dt.Columns.Add("FuncHref", System.Type.GetType("System.String"));
                //dt.Columns.Add("FuncDesc", System.Type.GetType("System.String"));
                //DataRow dr = dt.NewRow();
                //dr["FuncHref"] = "WorkStation.WorkStationCtl";
                //dr["FuncDesc"] = "WorkStation";
                //CUserControl cu = this.ShowTabPage("WorkStation", "刷码工作站", dr, null, null);
                ////cu.SetSetupParam = SP;
                ////cu.Tag = SP;
                //if (!this.tabControl.Visible)
                //    this.tabControl.Visible = true;
                this.WindowState = FormWindowState.Maximized;

                this.Text = "  " + ConfigurationManager.AppSettings["MainFormText"] + "    版本：" + ConfigurationManager.AppSettings["VersionMain"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //ShowWindow(FindWindow("Shell_TrayWnd", null), 9);
            }
        }
        #endregion

        #region BackGroudColor
        /// <summary>
        /// 存取系统背景色
        /// </summary>
        private System.Drawing.Color m_defaultBgColor = System.Drawing.Color.Transparent;
        /// <summary>
        /// 获取系统默认的背景色
        /// </summary>
        public System.Drawing.Color DefaultBgColor
        {
            get
            {
                if (m_defaultBgColor == System.Drawing.Color.Transparent)
                {
                    object rgbStr = ConfigurationManager.AppSettings["RGBForMainForm"];
                    if (rgbStr == null)
                        m_defaultBgColor = System.Drawing.Color.FromArgb(224, 224, 224);
                    else
                        m_defaultBgColor = StyleHelper.Instance.GetRGBFromStr(rgbStr.ToString());
                }
                return m_defaultBgColor;
            }
        }
        #endregion

        #region ClearFlozenColumn
        /// <summary>
        /// 递归算法，去除给定控件组中的DataGridView冻结列
        /// </summary>
        private void ClearFlozenColumn(System.Windows.Forms.Control.ControlCollection cc)
        {
            if (cc == null || cc.Count == 0) return;

            foreach (Control c in cc)
            {
                if (c is DataGridView)
                {
                    DataGridView dgv = c as DataGridView;
                    if (dgv.ReadOnly == false) dgv.RefreshEdit();
                    if (dgv.Columns.Count == 0) continue;
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Frozen)
                            col.Frozen = false;
                    }
                }
                else
                {
                    this.ClearFlozenColumn(c.Controls);
                }
            }
        }
        #endregion

        #region ShowForm 打开Form窗口
        /// <summary>
        /// 打开Form窗口
        /// </summary>
        /// <param name="Id">唯一识别号</param>
        /// <param name="text">标题</param>
        /// <param name="dr">对应的配置信息</param>
        /// <param name="strParam">字符串参数,参数格式为：“ParamName1=ParamValue1;............ParamNameN=ParamValueN”</param>
        /// <param name="dsParam">数据集参数</param>
        /// <returns>返回当前打开的窗口</returns>
        protected CForm ShowForm(string Id, string text, DataRow dr, string strParam, DataSet dsParam)
        {
            //FuncDesc用来传默认的参数
            string param1 = dr["FuncDesc"].ToString();
            if (string.IsNullOrEmpty(strParam))
                param1 = (param1 == "" ? "" : param1);
            else
                param1 = (param1 == "" ? strParam : param1 + ";" + strParam);

            Object obj = CObjectFactory.Instance.CreateObject(dr["FuncHref"].ToString().Trim(), "DLL");
            if (obj == null)
            {
                MessageBox.Show("CS_NOPARAM_FOR_OPENFORM");
                return null;
            }
            else
            {
                CForm frm = (CForm)obj;
                frm.Tag = Id;
                frm.Text = text;
                frm.BackColor = this.DefaultBgColor;
                frm.SetParam(param1, dsParam);
                frm.ShowDialog();
                return frm;
            }
        }
        #endregion

        #region CloseTabPage ICloseTabPage 成员
        public bool CloseTabPage(TabPage tp)
        {
            //检查打开的界面数据是否被修改，如果修改则应提示用户保存
            CUserControl uc = tp.Controls[0] as CUserControl;
            this.ClearFlozenColumn(tp.Controls);
            //在激活链中删除
            this.TagPagesActive.Remove(tp);
            //显示最前一次选择的tab页
            if (this.TagPagesActive.Count >= 1)
                this.tabControl.SelectTab((TabPage)this.TagPagesActive[this.TagPagesActive.Count - 1]);

            this.tabControl.TabPages.Remove(tp);
            //tp.Dispose();

            if (this.tabControl.TabPages.Count == 0)
                this.tabControl.Visible = false;
            GC.Collect();
            return true;
        }
        #endregion

        #region ShowTabPage 显示Id对应的用户组件
        /// <summary>
        /// 显示Id对应的ShowTabPage
        /// </summary>
        /// <param name="Id">唯一识别号</param>
        /// <param name="text">标题</param>
        /// <param name="dr">对应的配置信息</param>
        /// <param name="strParam">字符串参数,参数格式为：“ParamName1=ParamValue1;............ParamNameN=ParamValueN”</param>
        /// <param name="dsParam">数据集参数</param>
        /// <returns>返回当前打开的用户组件</returns>
        private CUserControl ShowTabPage(string Id, string text, DataRow dr, string strParam, DataSet dsParam)
        {
            //FuncDesc用来传默认的参数
            string param1 = dr["FuncDesc"].ToString();
            if (string.IsNullOrEmpty(strParam))
                param1 = (param1 == "" ? "" : param1);
            else
                param1 = (param1 == "" ? strParam : param1 + ";" + strParam);
            //添加参数OpenFromConsole，是否从主控台进入

            TabPage tp = null;
            if (this.tabControl.TabPages.Count > 0)
            {
                foreach (TabPage tpg in this.tabControl.TabPages)
                {
                    if (tpg.Tag.ToString() == Id)
                    {
                        tp = tpg;
                        break;
                    }
                }
            }
            if (tp != null)
            {
                this.tabControl.SelectedTab = tp;
                this.tabControl.SelectedTab.Focus();
                CUserControl uc = (CUserControl)(tp.Controls[0]);
                uc.SetParam(param1, dsParam);
                return uc;
            }

            object obj = CObjectFactory.Instance.CreateObject(dr["FuncHref"].ToString().Trim(), "DLL");
            if (obj != null)
            {
                CUserControl uc = (CUserControl)obj;
                uc.Dock = DockStyle.Fill;
                uc.Tag = Id;
                uc.BackColor = this.DefaultBgColor;
                uc.SetParam(param1, dsParam);
                tp = new TabPage();
                tp.Tag = Id;
                tp.Text = text;
                tp.BackColor = Color.FromArgb(238, 247, 252);
                tp.Controls.Add(uc);
                this.tabControl.TabPages.Add(tp);
                return uc;
            }
            else
            {
                MessageBox.Show("控件窗体加载失败！");
                return null;
            }
        }
        #endregion

        #region frmMainClient_FormClosing 窗口关闭时，应先检查打开的界面数据是否被修改，如果修改则应提示用户保存
        private void frmMainClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_UserControl.Closing();
            ShowWindow(FindWindow("Shell_TrayWnd", null), 9);
        }
        #endregion

    }
}
