using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BaseModel
{
    public interface IGotoForm
    {
        /// <summary>
        /// 切换到指定识别号的界面
        /// </summary>
        /// <param name="Id">唯一识别号</param>
        /// <param name="strParam">字符串参数,参数格式为：“ParamName1=ParamValue1;............ParamNameN=ParamValueN”</param>
        /// <param name="dsParam">数据集参数</param>
        /// <returns>返回打开的用户组件（CUserControl）或窗口实例（CForm）</returns>
        object GotoForm(string Id, string param, DataSet dsParam);
    }

    public interface ICloseTabPage
    {
        /// <summary>
        /// 关闭Tab页
        /// 修改人：Guyf
        /// 修改时间：2008.6.17
        /// </summary>
        /// <param name="tp">需要关闭的Tab页</param>
        bool CloseTabPage(System.Windows.Forms.TabPage tp);
    }
    public interface IReportAddMethod
    {
        /// <summary>
        /// 加入control的ID，用于报表
        /// 修改人：kzd
        /// 修改时间：2008.9.26
        /// </summary>

        void AddMethod(string key, object obj);
    }
}
