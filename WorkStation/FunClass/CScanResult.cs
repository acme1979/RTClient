using System;
using System.Collections.Generic;
using System.Text;

namespace WorkStation
{
    public class CScanResult
    {
        /// <summary>
        /// 条码值
        /// </summary>
        public string BarString { get; set; }
        /// <summary>
        /// 结果代码
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 提示原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 下一步操作提示
        /// </summary>
        public string ScanHint { get; set; }

        public CScanResult()
        { }
        public CScanResult(string barstring, string result, string remark, string scanhint)
        {
            BarString = barstring;
            Result = result;
            Remark = remark;
            ScanHint = scanhint;
        }
    }

    public class AssembleProductInfo
    {
        private string m_RecId;
        private string m_ProductSource;
        private string m_BarFormatd;

        public string RecId { get { return m_RecId; } }
        public string ProductSource { get { return m_ProductSource; } }
        public string BarFormat { get{return m_BarFormatd;} }

        public string BarString { get; set; }
        public AssembleProductInfo(string recid, string productSource, string barFormat)
        {
            m_RecId = recid;
            m_ProductSource = productSource;
            m_BarFormatd = barFormat;
            BarString = "";
        }
    }
}
