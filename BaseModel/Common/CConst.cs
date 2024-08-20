using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace BaseModel
{
    public class CConst
    {
        /// <summary>
        /// 系统配置信息对应的数据库代码
        /// </summary>
        public const string SetupInfoDBCode = "SetupInfoDBCode";
        /// <summary>
        /// 对应的数据库类型
        /// </summary>
        public const EDBType SetupInfoDBType = EDBType.SQLite;
        /// <summary>
        /// 数据库文件的位置及名称
        /// </summary>
        public static string SetupInfoDBPath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + @"\Config\Setup.dat";
            }
        }
    }

    /// <summary>
    /// 操作类型常量（新增、修改）
    /// </summary>
    /// <remarks>
    /// 创建人：Koulr
    /// 创建日期：2008-04-18
    /// </remarks>
    public class CAction
    {
        //新增操作
        public const string New = "New";
        //删除操作
        public const string Delete = "Delete";
        //修改操作
        public const string Modify = "Modify";
        // 查看操作
        public const string Query = "Query";
    }
    /// <summary>
    /// 系统支持的连接数据库类型
    /// </summary>
    public enum EDBType { 
        SQLServer = 1, 
        PGSQL = 2, 
        ORACLE = 3,
        ACCESS = 4,
        SQLite = 5,
        NoSQL = 6,
        MySQL = 7
    }

    


    #region DBParameters
///// <summary>
//    /// 存储过程参数类型
//    /// </summary>
//    public enum parameterDirections { Input, InputOutput, Output };
    /// <summary>
    /// 存储过程参数值类型
    /// </summary>
    public enum valueTypes { INT, STRING };
    /// <summary>
    /// 存储过程参数设置
    /// </summary>
    public struct DBParameters
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; }
        /// <summary>
        /// 参数输入输出类型
        /// </summary>
        public ParameterDirection parameterDirection { get; set; }
        /// <summary>
        /// 参数值数据类型
        /// </summary>
        public valueTypes ValueType { get; set; }
    }
    #endregion

    /// <summary>
    /// 读取文件方式（数据库，指定文件名，读取文件夹(文件以产品ID命名)）
    /// </summary>
    public enum EReadFileMode
    {
        /// <summary>
        /// 读取数据库
        /// </summary>
        [Description("读取数据库")]
        ReadDB = 1,
        /// <summary>
        /// 读取指定目录
        /// </summary>
        [Description("读取指定目录")]
        ReadDir = 2,
        /// <summary>
        /// 读取指定文件
        /// </summary>
        [Description("读取指定文件")]
        ReadFile = 3,
        /// <summary>
        /// 读取欧姆龙PLC
        /// </summary>
        [Description("读取欧姆龙PLC")]
        ReadOmronPLC = 4,
        /// <summary>
        /// 读取指定目录(品名)
        /// </summary>
        [Description("读取指定目录(品名)")]
        ReadDirPNAME = 5
    }
    /// <summary>
    /// 系统支持读取的文件类型
    /// </summary>
    public enum EFileType
    {
        Excel = 1,
        CSV = 2,
        TEXT = 3
    }
    /// <summary>
    /// 文件解析方式
    /// </summary>
    public enum EFileAnalysisType
    {
        [Description("对应SQL列")]
        ForSQL = 1,
        [Description("指定解析类")]
        ForClass = 2
    }
}
