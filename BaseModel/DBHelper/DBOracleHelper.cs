using System;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections;


namespace BaseModel
{
    class DBOracleSqlHelper : IDBHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SERVER">服务器地址及实例名</param>
        /// <param name="DBNAME">数据库名称</param>
        /// <param name="UserID">数据库登陆名</param>
        /// <param name="Password">数据库登陆密码</param>
        /// <param name="PORT">Oracle端口号，默认为5433</param>
        public DBOracleSqlHelper(string SERVER, string DBNAME, string UserID, string Password, string PORT = "1521")
        {
            try
            {
                if (sqlConn == null || sqlConn.State != System.Data.ConnectionState.Open)
                {
                    string strCon = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={4}))(CONNECT_DATA=(SERVICE_NAME={1})));User Id={2};Password={3};pooling=false",
                        SERVER, DBNAME, UserID, Password, PORT);
                    OracleConnection conn = new OracleConnection(strCon);
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
        private OracleConnection sqlConn = null;
        #endregion


        #region GetDataTable
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="queryString">查询语句</param>
        /// <param name="tableName">返回结果数据表名</param>
        public DataTable GetDataTable(string queryString, string tableName)
        {
            DataTable result = new DataTable();
            using (OracleDataAdapter sqlAda = new OracleDataAdapter(queryString, sqlConn))
            {
                try
                {
                    sqlAda.SelectCommand.CommandTimeout = 1000;
                    sqlAda.Fill(result);
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL执行失败！" + ex.Message);
                }
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
            int result = 0;
            using (OracleCommand sqlCmd = new OracleCommand(strSql, sqlConn))
            {
                try
                {
                    result = sqlCmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL执行失败！" + ex.Message);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBParameters"></param>
        /// <param name="funName"></param>
        /// <returns></returns>
        public Hashtable htExecuteNonQuery(List<DBParameters> DBParameters, string funName)
        {
            using (OracleCommand cmd = sqlConn.CreateCommand())
            {
                try
                {
                    Hashtable ht = new Hashtable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = funName;

                    foreach (var oracleParameter in DBParameters)
                    {
                        if (oracleParameter.parameterDirection == ParameterDirection.Input)
                        {
                            if (oracleParameter.ValueType == valueTypes.STRING)
                            {
                                cmd.Parameters.Add(oracleParameter.ParameterName, OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                            }
                            else if (oracleParameter.ValueType == valueTypes.INT)
                            {
                                cmd.Parameters.Add(oracleParameter.ParameterName, OracleDbType.Int32).Direction = ParameterDirection.Input;
                            }
                            else
                            {
                                cmd.Parameters.Add(oracleParameter.ParameterName, OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                            }
                            cmd.Parameters[oracleParameter.ParameterName].Value = oracleParameter.ParameterValue;
                        }
                        else if (oracleParameter.parameterDirection == ParameterDirection.InputOutput)
                        {
                            if (oracleParameter.ValueType == valueTypes.STRING)
                            {
                                cmd.Parameters.Add(oracleParameter.ParameterName, OracleDbType.Varchar2, 500).Direction = ParameterDirection.InputOutput;
                            }
                            else if (oracleParameter.ValueType == valueTypes.INT)
                            {
                                cmd.Parameters.Add(oracleParameter.ParameterName, OracleDbType.Int32).Direction = ParameterDirection.Input;
                            }
                            else
                            {
                                cmd.Parameters.Add(oracleParameter.ParameterName, OracleDbType.Varchar2, 500).Direction = ParameterDirection.Input;
                            }
                            cmd.Parameters[oracleParameter.ParameterName].Value = oracleParameter.ParameterValue;
                            
                        }
                        else
                        {
                            cmd.Parameters.Add(oracleParameter.ParameterName, OracleDbType.Varchar2,500).Direction = ParameterDirection.Output;
                        
                        }
                    }

                    //增加断连接操作
                    cmd.ExecuteNonQuery();//执行存储过程

                    foreach (var Parameter in DBParameters)
                    {
                        if (Parameter.parameterDirection == ParameterDirection.Input )
                        {
                            continue;
                        }
                        ht.Add(Parameter.ParameterName, cmd.Parameters[Parameter.ParameterName].Value.ToString());
                    }

                    cmd.Parameters.Clear();
                    return ht;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }


        #endregion

        #region 当前打开的是哪个数据库,对应INI文件中Section,即中括号内的代码
        /// <summary>
        /// 当前打开的是哪个数据库,对应INI文件中Section,即中括号内的代码
        /// </summary>
        private string m_DBCode;
        public string DBCode
        {
            get {  return m_DBCode;  }
            set { m_DBCode = value;  }
        }
        #endregion

        #region 获取当前数据库是哪个类别
        /// <summary>
        /// 获取当前数据库是哪个类别
        /// </summary>
        private EDBType m_DBTypeCode;
        public EDBType DBTypeCode
        {
            get{ return m_DBTypeCode; }
            set{ m_DBTypeCode = value; }
        }
        #endregion

        #region 判断数据库是否打开 没什么用
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
