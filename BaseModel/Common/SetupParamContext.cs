using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace BaseModel
{
    /// <summary>
    /// INI文件中，键值对存放结构体
    /// </summary>
    public class SetupParamContext
    {
        /// <summary>
        /// 节点名
        /// </summary>
        private string _Section;
        public string Section
        {
            get
            {
                return _Section;
            }
        }
        /// <summary>
        /// 键值名
        /// </summary>
        private string _Key;
        public string Key
        {
            get
            {
                return _Key;
            }
        }
        /// <summary>
        /// 值的内容
        /// </summary>
        private string _Value;
        public string Value
        {
            get
            {
                return _Value;
            }
        }
        /// <summary>
        /// Value值的内容是否有修改
        /// </summary>
        private bool _ModifyState;
        public bool ModifyState
        {
            get
            {
                return _ModifyState;
            }
        }
        #region 构造函数
        public SetupParamContext(string section, string key, string value)
        {
            _Section = section;
            _Key = key;
            _Value = value;
            _ModifyState = false;
        }
        #endregion

        #region 变更该节点内容
        /// <summary>
        /// 修改该节点的内容
        /// </summary>
        /// <param name="value">待变更的值</param>
        public void SetValue(string value)
        {
            if (_Value != value || value == "")
            {
                _Value = value;
                _ModifyState = true;
            }
        }
        #endregion
    }
}
    