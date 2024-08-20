﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace RSNClient
{
    public partial class CUserControl : UserControl
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
    }
}
