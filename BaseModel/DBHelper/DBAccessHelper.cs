using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace BaseModel
{
    public class DBAccessHelper : IDBHelper
    {
        #region 数据库连接器
        private OleDbConnection dbCon = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbFilePath">指定数据库文件</param>
        /// <param name="PWD">若该文件无密码,则传空字符串或者不传值</param>
        public DBAccessHelper(string dbFilePath, string PWD = "")
        {
            string dbConnString = "Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source=" + dbFilePath;
            if (PWD != "")
            {
                dbConnString = dbConnString + ";Jet OLEDB:Database Password=" + PWD;
            }
            if (dbCon == null || dbCon.State == ConnectionState.Closed)
            {
                dbCon = new OleDbConnection(dbConnString);
                dbCon.Open();
            }
        }
        #endregion

        #region 查询数据，返回数据集
        /// <summary>
        /// 使用SQL查询语句，获取数据集数据
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <returns></returns>
        public DataTable GetDataTable(string queryString, string tableName)
        {
            try
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(queryString, dbCon);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);
                dataset.Tables[0].TableName = tableName;
                return dataset.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 执行数据操作
        public int ExecuteSql(string strSql)
        {
            try
            {
                OleDbCommand command = new OleDbCommand(strSql, dbCon);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            dbCon.Close();
            dbCon = null;
        }
        #endregion
    }
}
