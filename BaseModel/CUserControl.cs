using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace BaseModel
{
    public partial class CUserControl : UserControl
    {
        protected string m_InputChar;
        public string InputChar
        {
            set
            {
                m_InputChar = value;
                InputCharChange();
            }
        }

        #region SystemType
        private string m_SystemType = string.Empty;
        /// <summary>
        /// 打开当前组件的子系统类型
        /// 默认在打开界面的时间由frmMain自动赋值，有需要的情况下子类可直接使用
        /// </summary>
        public string SystemType
        {
            get { return m_SystemType; }
            set { m_SystemType = value; }
        }
        #endregion

        #region ControlDesc
        private string m_ControlDesc = string.Empty;
        /// <summary>
        /// 当前组件的中文描述
        /// 默认在打开界面的时间由frmMain自动赋值，有需要的情况下子类可直接使用
        /// </summary>
        public string ControlDesc
        {
            get { return m_ControlDesc; }
            set { m_ControlDesc = value; }
        }
        #endregion

        protected virtual void InputCharChange()
        { }

        #region MainToolStrip
        /// <summary>
        /// 获取子类中的ToolStrip
        /// </summary>
        private ToolStrip m_MainToolStrip;
        /// <summary>
        /// 可在子类设计窗口赋值，也可在子类代码用赋值
        /// </summary>
        [Browsable(true), Category("Self-customized"), Description("容器中的ToolStrip"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ToolStrip MainToolStrip
        {
            get { return this.m_MainToolStrip; }
            set
            {
                this.m_MainToolStrip = value;
                if (!this.DesignMode)
                    StyleHelper.Instance.SetStyle(value);
            }
        }
        #endregion

        #region MainDataGridView
        /// <summary>
        /// 获取子类中的DataGridView
        /// </summary>
        private DataGridView m_MainDataGridView;
        /// <summary>
        /// 可在子类设计窗口赋值，也可在子类代码用赋值
        /// </summary>
        [Browsable(true), Category("Self-customized"), Description("容器中的DataGridView"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DataGridView MainDataGridView
        {
            get { return m_MainDataGridView; }
            set
            {
                if (m_MainDataGridView == value) return;
                m_MainDataGridView = value;
                if (!this.DesignMode)
                {
                    StyleHelper.Instance.SetStyle(value);
                    if (value != null && !value.ReadOnly)
                        StyleHelper.Instance.SetInputLength(value);
                }
            }
        }
        #endregion

        #region DsParam
        /// <summary>
        /// 存储外部调用时传入的DataSet
        /// </summary>
        private DataSet m_dsParam;
        /// <summary>
        /// 存取外部调用时传入的DataSet
        /// </summary>
        [Browsable(false)]
        protected DataSet DsParam
        {
            get
            {
                if (this.m_dsParam == null)
                    this.m_dsParam = new DataSet();
                return m_dsParam;
            }
            set
            {
                m_dsParam = value;
            }
        }
        #endregion

        #region Param
        /// <summary>
        /// 存储外部调用参数
        /// </summary>
        protected Hashtable m_ParamCache;
        /// <summary>
        /// 获取外部调用参数，参数格式：“ParamName1=ParamValue1;............ParamNameN=ParamValueN”
        /// </summary>
        [Browsable(false)]
        public string Param
        {
            set
            {
                if (m_ParamCache == null)
                    m_ParamCache = new Hashtable();
                else
                    m_ParamCache.Clear();

                if (!string.IsNullOrEmpty(value))
                {
                    string[] items = value.Split(';');
                    foreach (string item in items)
                    {
                        string temItem = item.Trim();
                        int pos = temItem.LastIndexOf('=');
                        if (pos > 0 && pos < temItem.Length - 1)
                            m_ParamCache[temItem.Substring(0, pos).Trim()] = temItem.Substring(pos + 1).Trim();
                    }
                }
            }
            get
            {
                if (this.m_ParamCache == null)
                    return "";
                string str = "";
                foreach (object obj in this.m_ParamCache.Keys)
                {
                    str += obj.ToString() + "=" + this.m_ParamCache[obj].ToString() + ";";
                }
                return str;
            }
        }
        #endregion

        public CUserControl()
        {
            InitializeComponent();
        }


        #region SetParam(string strParam, DataSet dsParam)
        public void SetParam(string strParam, DataSet dsParam)
        {
            this.Param = strParam;
            this.DsParam = dsParam;
            this.Active();
        }
        #endregion

        #region Active()
        /// <summary> 
        /// 当窗口在带参数的情况下被打开时触发，可重载
        /// </summary>
        public virtual void Active()
        {
        }

        #endregion

        #region
        /// <summary>
        /// 组件关闭时的统一处理方法
        /// </summary>
        protected  virtual void Close()
        {
            TabPage tp = (TabPage)this.Parent;
            if (this.ParentForm is ICloseTabPage)
            {
                ICloseTabPage frm = (ICloseTabPage)this.ParentForm;
                frm.CloseTabPage(tp);
            }
        }
        #endregion

        #region IntTextBox_KeyPress
        public void IntTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
        #endregion

        #region StringTextBox_KeyPress
        public void StringTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsLetter(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
        #endregion

        #region IntStringTextBox_KeyPress
        public void IntStringTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsLetter(e.KeyChar)) && !(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
        #endregion

        /// <summary>
        /// 当控件母窗体关闭时，调用此方法，可重载
        /// </summary>
        public virtual void Closing()
        {

        }
    }
}
