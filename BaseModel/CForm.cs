using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace BaseModel
{
    public partial class CForm : Form
    {

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

        #region 返回数据集
        private DataSet dsResult;
        public DataSet DsResult
        {
            get
            {
                if (dsResult == null)
                {
                    return new DataSet();
                }
                else
                {
                    return null;
                }
            }
            set
            { dsResult = value; }
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

        public CForm()
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
    }
}
