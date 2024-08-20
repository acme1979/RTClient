using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Collections;

namespace BaseModel
{
    public class DBSQLiteHelper : IDBHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbFilePath">指定数据库文件</param>
        public DBSQLiteHelper(string dbFilePath)
        {
            try
            {
                if (sqlConn == null || sqlConn.State != System.Data.ConnectionState.Open)
                {
                    sqlConn = SQLiteHelper.CreateNewDataBase(dbFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据库连接失败:" + ex.Message);
            }
        }
        #endregion

        #region 数据库连接
        private SQLiteConnection sqlConn = null;
        #endregion

        #region 当前打开的是哪个数据库,对应INI文件中Section,即中括号内的代码
        /// <summary>
        /// 当前打开的是哪个数据库,对应INI文件中Section,即中括号内的代码
        /// </summary>
        private string m_DBCode;
        public string DBCode
        {
            get
            {
                return m_DBCode;
            }
            set
            {
                m_DBCode = value;
            }
        }
        #endregion
        
        #region 获取当前数据库是哪个类别
        /// <summary>
        /// 获取当前数据库是哪个类别
        /// </summary>
        private EDBType m_DBTypeCode;
        public EDBType DBTypeCode
        {
            get
            {
                return m_DBTypeCode;
            }
            set
            {
                m_DBTypeCode = value;
            }
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="queryString">查询语句</param>
        /// <param name="tableName">返回结果数据表名</param>
        public DataTable GetDataTable(string queryString, string tableName)
        {
            DataSet ds = SQLiteHelper.ExecuteDataSet(sqlConn, queryString, null);
            ds.Tables[0].TableName = tableName;
            return ds.Tables[tableName];
        }
        #endregion

        #region ExecuteSql
        /// <summary>
        /// 执行SQL脚本
        /// </summary>
        /// <param name="strSql">Sql脚本</param>
        /// <returns>影响的数据行数</returns>
        public int ExecuteSql(string queryString)
        {
            return SQLiteHelper.ExecuteNonQuery(sqlConn, queryString, null);
        }
        #endregion

        #region 参数设置
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="ListDBParameters"></param>
        /// <param name="ParameterName"></param>
        /// <param name="ParameterValue"></param>
        /// <param name="parameterDirection"></param>
        /// <param name="ValueType"></param>
        public void AddDBParameters(List<DBParameters> ListDBParameters, string ParameterName, string ParameterValue, ParameterDirection parameterDirection, valueTypes ValueType)
        {
            DBParameters DBParameter = new DBParameters();
            DBParameter.ParameterName = ParameterName;
            DBParameter.ParameterValue = ParameterValue;
            DBParameter.parameterDirection = parameterDirection;
            DBParameter.ValueType = ValueType;
            ListDBParameters.Add(DBParameter);
        }
        #endregion


        #region 存储过程
        public Hashtable htExecuteNonQuery(List<DBParameters> DBParameters, string funName)
        {
            return new Hashtable();
        }
        #endregion

        #region 判断数据库是否打开
        public bool DBISOPEN()
        {
            return true;
        }
        #endregion

        #region 销毁该对象
        /// <summary>
        /// 销毁该对象
        /// </summary>
        public void Destroy()
        {
            sqlConn.Close();
            sqlConn = null;
        }
        #endregion
    }
}
