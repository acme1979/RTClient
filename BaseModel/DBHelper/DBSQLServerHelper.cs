using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections;
using System.Configuration;
using ConfigHelper;
using System.Windows.Forms;

namespace BaseModel
{
    public class DBSQLServerHelper : IDBHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SERVER">服务器地址及实例名</param>
        /// <param name="DBNAME">数据库名称</param>
        /// <param name="UserID">数据库登陆名</param>
        /// <param name="Password">数据库登陆密码</param>
        public DBSQLServerHelper(string SERVER, string DBNAME, string UserID, string Password, string PORT="1433")
        {
            try
            {
                if (sqlConn == null || sqlConn.State != ConnectionState.Open)
                {
                    string strCon = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};",
                        SERVER, DBNAME, UserID, Password);
                    SqlConnection conn = new SqlConnection(strCon);
                    conn.Open();
                    sqlConn = conn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据库连接失败:" + ex.Message);
            }
        }
        #endregion

        #region 数据库连接
        private SqlConnection sqlConn = null;
        #endregion

        #region GetDataTable
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="queryString">查询语句</param>
        /// <param name="tableName">返回结果数据表名</param>
        public DataTable GetDataTable(string queryString,string tableName)
        {
            DataTable result = new DataTable();
            using (SqlDataAdapter sqlAda = new SqlDataAdapter(queryString, sqlConn))
            {
                sqlAda.SelectCommand.CommandTimeout = 1000;
                sqlAda.Fill(result);
            }
            result.TableName = tableName;
            return result;
        }
        #endregion

        #region ExecuteSql
        /// <summary>
        /// 执行SQL脚本
        /// </summary>
        /// <param name="strSql">Sql脚本</param>
        /// <returns>影响的数据行数</returns>
        public int ExecuteSql(string strSql)
        {
            using (SqlCommand sqlCmd = new SqlCommand(strSql, sqlConn))
            {
                try
                {
                    int result = 0;
                    result = sqlCmd.ExecuteNonQuery();
                    return result;
                }
                catch
                {
                    throw new Exception("SQL执行异常！");
                }
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

        #region 销毁该对象
        /// <summary>
        /// 销毁该对象
        /// </summary>
        public void Destroy()
        {
            sqlConn.Close();
        }
        #endregion

        #region 判断数据库是否打开
        public bool DBISOPEN()
        {
            return true;
        }
        #endregion
    }
}
