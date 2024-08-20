using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
//using AutoMachineDataRead;
using BaseModel;

namespace RTClient
{
    public partial class MainFrm : Form, IGotoForm, ICloseTabPage
    {
        #region
        private string m_SystemType;
        private CUserControl opCtl;
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

        //存取主控界面
        private Form m_consoleForm = null;

        public MainFrm(string SystemType)
        {
            InitializeComponent();
            m_SystemType = SystemType;
            this.Text = ConfigurationManager.AppSettings["MainFormText"];
            this.InitCtl();
        }
        
        #region InitCtl 初始化设置
        /// <summary>
        /// 初始化设置
        /// </summary>
        private void InitCtl()
        {
            this.menuStrip.CanOverflow = true;
            this.menuStrip.Items.Clear();
            bool DBCon = Convert.ToBoolean(ConfigurationManager.AppSettings["DBConnection"]);
            bool b = false;
            if (DBCon)
            {
                try
                {
                    this.InitMenu(this.menuStrip.Items, "");
                    b = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库连接失败,请重新设置!");
                }
                //初始化关于菜单
                this.InitAboutMenu();
            }
            else
            {
                //InitFunction();
                InitMenu(this.menuStrip.Items);
            }

            //添加注销按钮
            ToolStripMenuItem logoutMenuItem = new ToolStripMenuItem();
            logoutMenuItem.Text = "注销";
            logoutMenuItem.Alignment = ToolStripItemAlignment.Right;
            logoutMenuItem.AutoSize = false;
            logoutMenuItem.Width = 100;
            logoutMenuItem.Height = 25;
            logoutMenuItem.ForeColor = Color.Red;
            logoutMenuItem.BackgroundImage = global::RTClient.Properties.Resources.Menu2px;
            logoutMenuItem.BackgroundImageLayout = ImageLayout.Stretch;
            logoutMenuItem.Overflow = ToolStripItemOverflow.AsNeeded;
            this.menuStrip.Items.Add(logoutMenuItem);
            logoutMenuItem.Click += new EventHandler(logoutMenuItem_Click);

            //添加注销按钮
            ToolStripMenuItem setupMenuItem = new ToolStripMenuItem();
            setupMenuItem.Text = "设置";
            setupMenuItem.Alignment = ToolStripItemAlignment.Right;
            setupMenuItem.AutoSize = false;
            setupMenuItem.Width = 100;
            setupMenuItem.Height = 25;
            setupMenuItem.ForeColor = Color.Red;
            setupMenuItem.BackgroundImage = global::RTClient.Properties.Resources.Menu2px;
            setupMenuItem.BackgroundImageLayout = ImageLayout.Stretch;
            setupMenuItem.Overflow = ToolStripItemOverflow.AsNeeded;
            this.menuStrip.Items.Add(setupMenuItem);

            ToolStripMenuItem DBfuncMenuItem = new ToolStripMenuItem();
            DBfuncMenuItem.Tag = "DBSetup";
            DBfuncMenuItem.Text = "数据库设置";
            DBfuncMenuItem.Click += new EventHandler(MenuItemDBSetup_Click);
            setupMenuItem.DropDownItems.Add(DBfuncMenuItem);

            if (m_SystemType == "MachineDataAutoRead")
            {
                ToolStripMenuItem GeneralSetupMenuItem = new ToolStripMenuItem();
                GeneralSetupMenuItem.Tag = "GeneralSetup";
                GeneralSetupMenuItem.Text = "常规参数设置";
                GeneralSetupMenuItem.Click += new EventHandler(MenuItemGeneralSetup_Click);
                setupMenuItem.DropDownItems.Add(GeneralSetupMenuItem);
            }

            //设置欢迎词
            if(b)
            {
                ToolStripMenuItem welcomeMenuItem = new ToolStripMenuItem();
                welcomeMenuItem.Text = UserInfo.Instance.UserName + " 欢迎您！";
                welcomeMenuItem.Alignment = ToolStripItemAlignment.Right;
                welcomeMenuItem.Enabled = false;
                this.menuStrip.Items.Add(welcomeMenuItem);
            }

            this.Load += new EventHandler(MainFrm_Load);
            this.FormClosing += new FormClosingEventHandler(frmMain_FormClosing);
            this.FormClosed += new FormClosedEventHandler(frmMain_FormClosed);
            this.tabControl.SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged);
        }
        #endregion

        #region InitAboutMenu 添加关于菜单
        /// <summary>
        /// 添加关于菜单
        /// </summary>
        private void InitAboutMenu()
        {
            ToolStripMenuItem tsmiAbouts = new ToolStripMenuItem();
            tsmiAbouts.Text = "系统设定";
            tsmiAbouts.Width = tsmiAbouts.Text.Length * 15 + 10;
            tsmiAbouts.Width = 25;
            tsmiAbouts.BackgroundImage = global::RTClient.Properties.Resources.Menu2px;
            tsmiAbouts.BackgroundImageLayout = ImageLayout.Stretch;
            tsmiAbouts.Overflow = ToolStripItemOverflow.AsNeeded;
            this.menuStrip.Items.Add(tsmiAbouts);

            //添加数据库设置子菜单
            ToolStripMenuItem tsmiDBSetting = new ToolStripMenuItem();
            tsmiDBSetting.Text = "数据库设置";
            tsmiAbouts.DropDownItems.Add(tsmiDBSetting);
            tsmiDBSetting.Click += new EventHandler(tsmiDBSetting_Click);

            //添加关于子菜单
            ToolStripMenuItem tsmiAbout = new ToolStripMenuItem();
            tsmiAbout.Text = "关于";
            tsmiAbouts.DropDownItems.Add(tsmiAbout);
            tsmiAbout.Click += new EventHandler(tsmiAbout_Click);

            //添加帮助子菜单
            if (ConfigurationManager.AppSettings["NeedHelpMenu"] != null && ConfigurationManager.AppSettings["NeedHelpMenu"].ToLower().Equals("true"))
            {
                ToolStripMenuItem tsmiHelp = new ToolStripMenuItem();
                tsmiHelp.Text = "帮助";
                tsmiAbouts.DropDownItems.Add(tsmiHelp);
                tsmiHelp.Click += new EventHandler(tsmiHelp_Click);
            }
        }
        #endregion

        #region InitMenuNoDB
        /// <summary>
        /// 单机版，无数据库连接菜单初始化
        /// </summary>
        /// <param name="pmi"></param>
        private void InitMenu(ToolStripItemCollection pmi)
        {
            //if (m_SystemType == "MachineDataAutoRead")
            //{
            //    DataTable dt = DBHelper.Instance.GetSetupDB().GetDataTable("select * from SETUPINFO", "SETUPINFO");
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        SetupParam sp = new SetupParam(dr["CODE"].ToString());
            //        ToolStripMenuItem groupMenuItem = new ToolStripMenuItem();
            //        groupMenuItem.Tag = sp;
            //        groupMenuItem.Text = dr["NAME"].ToString();
            //        groupMenuItem.Alignment = ToolStripItemAlignment.Left;
            //        groupMenuItem.AutoSize = false;
            //        groupMenuItem.Width = groupMenuItem.Text.Length * 15 + 10;
            //        groupMenuItem.Height = 25;
            //        groupMenuItem.BackgroundImage = global::RTClient.Properties.Resources.Menu2px;
            //        groupMenuItem.BackgroundImageLayout = ImageLayout.Stretch;
            //        groupMenuItem.Overflow = ToolStripItemOverflow.AsNeeded;
            //        groupMenuItem.Click += new EventHandler(MenuItemNoDB_Click);
            //        pmi.Add(groupMenuItem);
            //    }
            //}

            //groupMenuItem = new ToolStripMenuItem();
            //groupMenuItem.Tag = "ZDTPACKLABPRT";
            //groupMenuItem.Text = "臻鼎出货标签打印";
            //groupMenuItem.Alignment = ToolStripItemAlignment.Left;
            //groupMenuItem.AutoSize = false;
            //groupMenuItem.Width = groupMenuItem.Text.Length * 15 + 10;
            //groupMenuItem.Height = 25;
            //groupMenuItem.BackgroundImage = global::RTClient.Properties.Resources.Menu2px;
            //groupMenuItem.BackgroundImageLayout = ImageLayout.Stretch;
            //pmi.Add(groupMenuItem);

            //ToolStripMenuItem funcMenuItem = new ToolStripMenuItem();
            //funcMenuItem.Tag = "ZDTPACKLABBASEINFO";
            //funcMenuItem.Text = "基础信息维护";
            //funcMenuItem.Click += new EventHandler(MenuItemNoDB_Click);
            //groupMenuItem.DropDownItems.Add(funcMenuItem);
        }
        #endregion

        #region InitFunction 为单机版程序添加功能
        private void InitFunction()
        {
            if (m_SystemType == "ZonjaCarLabDataCrt")
            {
                DataTable dt = new DataTable("Templet");
                dt.Columns.Add("FuncHref", System.Type.GetType("System.String"));
                dt.Columns.Add("FuncDesc", System.Type.GetType("System.String"));
                DataRow dr = dt.NewRow();
                dr["FuncHref"] = "CartonLabDataCrt.CarLabDataCrtCtl";
                dr["FuncDesc"] = "装箱数据生成";
                this.ShowTabPage("ZonjaCarLabDataCrt", "装箱数据生成", dr, null, null);
                if (!this.tabControl.Visible)
                    this.tabControl.Visible = true;
            }
        }
        #endregion

        #region tsmiAbout_Click
        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            Form frm = new AboutBoxfrm();
            frm.ShowDialog();
        }
        #endregion

        #region tsmiDBSetting_Click
        private void tsmiDBSetting_Click(object sender, EventArgs e)
        {
            DBSetupFrm frm = new DBSetupFrm();
            frm.ShowDialog();
        }
        #endregion

        #region tsmiHelp_Click
        private void tsmiHelp_Click(object sender, EventArgs e)
        {
            //frmHelp frm = new frmHelp();
            //frm.BackColor = this.BackColor;
            //frm.ShowDialog();
        }
        #endregion

        #region logoutMenuItem_Click
        private void logoutMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            GC.Collect();
            Application.Exit();

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "RTClient.exe";
            System.Diagnostics.Process.Start(startInfo);
        }
        #endregion

        #region frmMain_FormClosing 窗口关闭时，应先检查打开的界面数据是否被修改，如果修改则应提示用户保存
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            //检查打开的界面数据是否被修改，如果修改则应提示用户保存
            for (int i = this.tabControl.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage tp = this.tabControl.TabPages[i];
                if (!this.CloseTabPage(tp))
                {
                    e.Cancel = true;
                    return;
                }
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

        #region tabControl_MouseClick
        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            bool showPopoMenu = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowPopoMenu"]);
            if (showPopoMenu)
            {
                if (e.Button == MouseButtons.Right && this.tabControl.TabPages.Count > 0)
                {
                    this.popupMenu.Show(this.tabControl, e.X, e.Y);
                }
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

        #region frmMain_FormClosed 窗口关闭时，显示主控台
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //窗口关闭时，显示主控台
            try
            {
                if (this.m_consoleForm != null)
                {
                    if (this.m_consoleForm.OwnedForms.Length <= 1)
                    {
                        this.m_consoleForm.Show();
                        if (this.m_consoleForm.WindowState == FormWindowState.Minimized)
                            this.m_consoleForm.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch { }
        }
        #endregion

        #region event handler for 当前的Tab页发生改变时,把当前页提到链表最右边
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                this.TagPagesActive.Remove(tabControl.SelectedTab);
                this.TagPagesActive.Add(tabControl.SelectedTab);
            }
        }
        #endregion
        
        #region InitMenu 初始化菜单
        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="ptn"></param>
        /// <param name="systemId"></param>k
        private bool InitMenu(ToolStripItemCollection pmi, string parentGroupId)
        {
            string strFilter = "ParentGroupId='" + parentGroupId + "'";
            DataRow[] drs = UserInfo.Instance.FuncDataSet.Tables["SYS_FuncGroup"].Select(strFilter);
            if (drs == null || drs.Length == 0)
                return false;

            foreach (DataRow dr in drs)
            {
                string isFunc = dr["IsFunc"].ToString();
                string groupId = dr["GroupId"].ToString().Trim();
                ToolStripMenuItem groupMenuItem = new ToolStripMenuItem();
                groupMenuItem.Tag = groupId;
                groupMenuItem.Text = dr["GroupNm"].ToString().Trim();
                if (parentGroupId == "")
                {
                    groupMenuItem.Alignment = ToolStripItemAlignment.Left;
                    groupMenuItem.AutoSize = false;
                    groupMenuItem.Width = groupMenuItem.Text.Length * 15 + 10;
                    groupMenuItem.Height = 25;
                    groupMenuItem.BackgroundImage = global::RTClient.Properties.Resources.Menu2px;
                    groupMenuItem.BackgroundImageLayout = ImageLayout.Stretch;
                    groupMenuItem.Overflow = ToolStripItemOverflow.AsNeeded;
                }
                if (isFunc == "1")
                    groupMenuItem.Click += new EventHandler(MenuItem_Click);
                else
                    this.InitMenu(groupMenuItem.DropDownItems, groupId);
                pmi.Add(groupMenuItem);
                //如果IsAddLine属性为1，则还要在当前菜单结点后添加分隔线
                if (dr["IsAddLine"].ToString().Trim() == "1")
                {
                    ToolStripSeparator lineItem = new ToolStripSeparator();
                    pmi.Add(lineItem);
                }
            }
            return true;
        }
        #endregion

        #region frmMain_Load 窗口打开时，设置相应的Banner及菜单工具条背景图
        private void MainFrm_Load(object sender, EventArgs e)
        {
            //if (RunningInstance() != null)
            //{
            //    MessageBox.Show("警告：程序已经被打开,该程序只能打开一个！！！");
            //    this.Close();
            //    return;
            //}
            string imagePath = Directory.GetCurrentDirectory() + "\\Images";
            //加载Banner
            string bannerImage = imagePath + "\\Banner" + m_SystemType + ".jpg";
            if (File.Exists(bannerImage))
                this.pictureBox1.BackgroundImage = Image.FromFile(bannerImage);
            else
                this.pictureBox1.BackgroundImage = global::RTClient.Properties.Resources.Banner;
            this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            //加载菜单背景图
            string menuImage = imagePath + "\\Menubg.jpg";
            if (File.Exists(menuImage))
                this.menuStrip.BackgroundImage = Image.FromFile(menuImage);
            else
                this.menuStrip.BackgroundImage = global::RTClient.Properties.Resources.Menubg;
            this.menuStrip.BackgroundImageLayout = ImageLayout.Stretch;

            //设置主操作区背景图
            string mainImage = imagePath + "\\Mainbg.jpg";
            if (File.Exists(mainImage))
            {
                this.pnlMain.BackgroundImage = Image.FromFile(mainImage);
                this.pnlMain.BackgroundImageLayout = ImageLayout.Stretch;
            }
            //LoadMachineDataAutoReadFunction();
            GC.Collect();
        }
        #endregion

        #region LoadMachineDataAutoReadFunction
        //private void LoadMachineDataAutoReadFunction()
        //{
        //    if (m_SystemType == "MachineDataAutoRead")
        //    {
        //        foreach (ToolStripMenuItem tsmi in this.menuStrip.Items)
        //        {
        //            SetupParam SP = tsmi.Tag as SetupParam;
        //            if (SP != null && SP.AutoStart)
        //            {
        //                try
        //                {
        //                    DataTable dt = new DataTable("Templet");
        //                    dt.Columns.Add("FuncHref", System.Type.GetType("System.String"));
        //                    dt.Columns.Add("FuncDesc", System.Type.GetType("System.String"));
        //                    DataRow dr = dt.NewRow();
        //                    dr["FuncHref"] = "AutoMachineDataRead.MachineDataAutoReadCtl";
        //                    dr["FuncDesc"] = SP.Name;
        //                    CUserControl cu = this.ShowTabPage(SP.Code, SP.Name, dr, null, null);
        //                    //cu.SetSetupParam = SP;
        //                    //cu.Tag = SP;
        //                    if (!this.tabControl.Visible)
        //                        this.tabControl.Visible = true;
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        #region MenuItem_Click 菜单项单击方法
        private void MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsMenuItem = (ToolStripMenuItem)sender;
            string Id = tsMenuItem.Tag.ToString();
            DataRow dr = UserInfo.Instance.FuncDataSet.Tables["SYS_Func"].Select("FuncId='" + Id + "'")[0];
            if (dr == null)
            {
                MessageBox.Show("未找到此功能信息！");
                return;
            }

            try
            {
                string funcHref = dr["FuncHref"].ToString().Trim();
                string[] val = funcHref.Split(',');
                if (val == null || val.Length == 0)
                {
                    MessageBox.Show("关联窗体的功能参数设置错误！");
                    return;
                }
                else
                {
                    val[0] = val[0].Trim();
                }

                if (val[0].Substring(val[0].Length - 3).ToLower() == "frm")
                    this.ShowForm(Id, tsMenuItem.Text, dr, null, null);
                else
                {
                    this.ShowTabPage(Id, tsMenuItem.Text, dr, null, null);
                    if (!this.tabControl.Visible)
                        this.tabControl.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region MenuItemGeneralSetup_Click 菜单项单击方法
        private void MenuItemGeneralSetup_Click(object sender, EventArgs e)
        {

            //GeneralSetupFrm frm = new GeneralSetupFrm();
            //frm.ShowDialog();
        }
        #endregion

        #region MenuItemNoDB_Click 菜单项单击方法MenuItemGeneralSetup_Click
        private void MenuItemDBSetup_Click(object sender, EventArgs e)
        {
            DBSetupFrm frm = new DBSetupFrm();
            frm.ShowDialog();
        }
        #endregion

        #region MenuItemNoDB_Click 菜单项单击方法
        private void MenuItemNoDB_Click(object sender, EventArgs e)
        {
            //ToolStripMenuItem tsMenuItem = (ToolStripMenuItem)sender;
            //SetupParam SP = tsMenuItem.Tag as SetupParam;

            //try
            //{
            //    DataTable dt = new DataTable("Templet");
            //    dt.Columns.Add("FuncHref", System.Type.GetType("System.String"));
            //    dt.Columns.Add("FuncDesc", System.Type.GetType("System.String"));
            //    DataRow dr = dt.NewRow();
            //    dr["FuncHref"] = "AutoMachineDataRead.MachineDataAutoReadCtl";
            //    dr["FuncDesc"] = SP.Name;
            //    this.ShowTabPage(SP.Code, SP.Name, dr, null, null);
            //    if (!this.tabControl.Visible)
            //        this.tabControl.Visible = true;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //ToolStripMenuItem tsMenuItem = (ToolStripMenuItem)sender;
            //string Id = tsMenuItem.Tag.ToString();

            //try
            //{
            //    if (Id == "STDataRead")
            //    {
            //        DataTable dt = new DataTable("Templet");
            //        dt.Columns.Add("FuncHref", System.Type.GetType("System.String"));
            //        dt.Columns.Add("FuncDesc", System.Type.GetType("System.String"));
            //        DataRow dr = dt.NewRow();
            //        dr["FuncHref"] = "STDataRead.DataReadCtl";
            //        dr["FuncDesc"] = "山田设备数据自动读取";
            //        this.ShowTabPage(Id, "山田设备数据自动读取", dr, null, null);
            //        if (!this.tabControl.Visible)
            //            this.tabControl.Visible = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
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
                opCtl = uc;
                return uc;
            }

            object obj = CObjectFactory.Instance.CreateObject(dr["FuncHref"].ToString().Trim(),"DLL");
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
                //由于前一句是加载页面的过程，而某些页面在加载的过程中因为某些因素导致加载不成功，这个时候tp不能添加到激活页面数组中
                if (this.tabControl.TabPages.Contains(tp))
                {
                    this.TagPagesActive.Add(tp);
                    this.tabControl.SelectedIndex = this.tabControl.TabPages.Count - 1;
                    this.tabControl.SelectedTab.Focus();
                }
                opCtl = uc;
                return uc;
            }
            else
            {
                MessageBox.Show("控件窗体加载失败！");
                return null;
            }
        }
        #endregion

        #region tsmCloseOther_Click
        private void tsmCloseOther_Click(object sender, EventArgs e)
        {
            TabPage tp1 = this.tabControl.SelectedTab;
            for (int i = this.tabControl.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage tp = this.tabControl.TabPages[i];
                if (tp1 != tp)
                {
                    if (!this.CloseTabPage(tp)) break;
                }

            }
            GC.Collect();
        }
        #endregion

        #region tsmCloseAll_Click
        private void tsmCloseAll_Click(object sender, EventArgs e)
        {
            for (int i = this.tabControl.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage tp = this.tabControl.TabPages[i];
                if (!this.CloseTabPage(tp)) break;
            }
            GC.Collect();
        }
        #endregion

        #region tsmClose_Click
        private void tsmClose_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab != null)
            {
                TabPage tp = this.tabControl.SelectedTab;
                this.CloseTabPage(tp);
            }
        }
        #endregion

        #region RunningInstance 获取正在运行的实例，没有运行的实例返回null
        /// <summary>
        /// 获取正在运行的实例，没有运行的实例返回null;
        /// </summary>
        public static System.Diagnostics.Process RunningInstance()
        {
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);
            foreach (System.Diagnostics.Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
