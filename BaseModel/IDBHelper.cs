using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;


namespace BaseModel
{
    public interface IDBHelper
    {


        /// <summary>
        /// 当前打开的是哪个数据库,对应INI文件中Section,即中括号内的代码
        /// </summary>
        string DBCode { get; set; }

        /// <summary>
        /// 获取当前数据库是哪个类别
        /// </summary>
        EDBType DBTypeCode { get; set; }

        /// <summary>
        /// 获取数据结果表
        /// </summary>
        /// <param name="queryString">字符串</param>
        /// <param name="tableName">表名</param>
        /// <returns>返回数据</returns>
        DataTable GetDataTable(string queryString, string tableName);

        ///// <summary>
        /////  执行有返回值的SQL语句
        ///// </summary>
        ///// <param name="queryString">字符串</param>
        ///// <param name="tableName">表名</param>
        ///// <returns>返回数据</returns>
        //DataSet LoadSQLData(string queryString, string[] tableName);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">待执行的SQL语句</param>
        /// <returns>返回受影响行数</returns>
        int ExecuteSql(string strSql);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListDBParameters"></param>
        /// <param name="ParameterName"></param>
        /// <param name="ParameterValue"></param>
        /// <param name="parameterDirection"></param>
        /// <param name="ValueType"></param>
        void AddDBParameters(List<DBParameters> ListDBParameters, string ParameterName, string ParameterValue, ParameterDirection parameterDirection, valueTypes ValueType);
   
        /// <summary>
        /// Hashtable形式存储过程执行方法
        /// </summary>
        /// <param name="date">输入输出参数集</param>
        /// <param name="funName">存储名称</param>
        /// <returns></returns>
        Hashtable htExecuteNonQuery(List<DBParameters> DBParameters, string funName);

        /// <summary>
        /// 判断数据库是否打开
        /// </summary>
        /// <returns></returns>
        bool DBISOPEN();

        /// <summary>
        /// 销毁该对象
        /// </summary>
        void Destroy();


    }
}
