using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using BaseModel;

namespace WorkStation
{
    interface ICommonWorkStationCtl
    {
        /// <summary>
        /// 当前制令单
        /// </summary>
        string PO { get; set; }
        /// <summary>
        /// 当前生产工序代码
        /// </summary>
        string Process { get; set; }
        /// <summary>
        /// 当前生产工序名称
        /// </summary>
        string ProcessName { get; set; }
        /// <summary>
        /// 当前生产线体
        /// </summary>
        string ProductLine { get; set; }
        /// <summary>
        /// 当前生产线体名称
        /// </summary>
        string ProductLineName { get; set; }
        /// <summary>
        /// 当前工作中心代码
        /// </summary>
        string WorkStation { get; set; }
        /// <summary>
        /// 当前工作中心名称
        /// </summary>
        string WorkStationName { get; set; }
        /// <summary>
        /// 当前用户工号
        /// </summary>
        string UsrCode { get; set; }
        /// <summary>
        /// 当前用户姓名
        /// </summary>
        string UsrName { get; set; }
        /// <summary>
        /// 界面显示模块列表
        /// </summary>
        List<string> GroupBoxShowList { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        string DBCode  { get; set; }
        /// <summary>
        /// 数据库链接
        /// </summary>
        IDBHelper iDBHelper  { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        string DATA_AUTH { get; set; }

        /// <summary>
        /// 系统控件对接的通用参数集合
        /// </summary>
        Hashtable HtCommonParam { get; set; }

        CScanResult ExecScanBarOperate(string BarString);

        /// <summary>
        /// 用于执行控件数据初始化；
        /// </summary>
        void InitData();
    }
}
