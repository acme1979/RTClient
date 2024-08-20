using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Data;
using System.Collections;

namespace BaseModel
{
    public class DBPGSQLHelper : IDBHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SERVER">服务器地址及实例名</param>
        /// <param name="DBNAME">数据库名称</param>
        /// <param name="UserID">数据库登陆名</param>
        /// <param name="Password">数据库登陆密码</param>
        /// <param name="PORT">PGSQL端口号，默认为5433</param>
        public DBPGSQLHelper(string SERVER, string DBNAME, string UserID, string Password, string PORT = "5432")
        {
            GetContext(SERVER,DBNAME,UserID,Password,PORT);
            //try
            //{
            //    if (sqlConn == null || sqlConn.State != System.Data.ConnectionState.Open)
            //    {
            //        string strCon = string.Format("PORT={4};DATABASE={1};HOST={0};PASSWORD={3};USER ID={2};",
            //            SERVER, DBNAME, UserID, Password, PORT);
            //        NpgsqlConnection conn = new NpgsqlConnection(strCon);
            //        conn.Open();
            //        sqlConn = conn;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("数据库连接失败:" + ex.Message);
            //}
        }
        #endregion

        #region 创建连接
        private void GetContext(string SERVER, string DBNAME, string UserID, string Password, string PORT = "5432")
        {
            try
            {
                if (sqlConn == null || sqlConn.State != System.Data.ConnectionState.Open)
                {
                    string strCon = string.Format("PORT={4};DATABASE={1};HOST={0};PASSWORD={3};USER ID={2};pooling=false",
                        SERVER, DBNAME, UserID, Password, PORT);
                    NpgsqlConnection conn = new NpgsqlConnection(strCon);
                    conn.Open();
                    sqlConn = conn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据库连接失败:" + ex.Message);
            }

        }

        private void getConn()
        {
            string SERVER = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "SERVER"));
            string DBNAME = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DBNAME"));
            string UserID = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "UserID"));
            string Password = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "Password"));
            string PORT = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "PORT"));
            GetContext(SERVER, DBNAME, UserID, Password, PORT);
        }
        #endregion

        #region 数据库连接
        private NpgsqlConnection sqlConn = null;
        #endregion

        #region 当前打开的是哪个数据库,对应INI文件中Section,即中括号内的代码
        /// <summary>
        /// 当前打开的是哪个数据库,对应INI文件中Section,即中括号内的代码
        /// </summary>
        private string m_DBCode;
        public string DBCode
        {
            get { return m_DBCode;  }
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
            get {  return m_DBTypeCode; }
            set {  m_DBTypeCode = value; }
        }
        #endregion

        #region 判断数据库是否打开  没什么用
        public bool DBISOPEN()
        {
            try
            {
                string sql = "SELECT * FROM DUAL";
                ExecuteSql(sql);
            }
            catch (Exception)
            {
                return false;
            }
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



        #region GetDataTable
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="queryString">查询语句</param>
        /// <param name="tableName">返回结果数据表名</param>
        public DataTable GetDataTable(string queryString,string tableName)
        {
            DataTable result = new DataTable();
            using (NpgsqlDataAdapter sqlAda = new NpgsqlDataAdapter(queryString, sqlConn))
            {
                try
                {
                    sqlAda.SelectCommand.CommandTimeout = 1000;
                    sqlAda.Fill(result);
                }
                catch (Exception ex)
                {
                    try
                    {
                        Destroy();
                        getConn();
                        using (NpgsqlDataAdapter sqlAda1 = new NpgsqlDataAdapter(queryString, sqlConn))
                        {
                            sqlAda1.SelectCommand.CommandTimeout = 1000;
                            sqlAda1.Fill(result);
                        }
                    }
                    catch (Exception ex1)
                    {
                        throw new Exception("SQL执行异常！" + ex1.Message);
                    }
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
            using (NpgsqlCommand sqlCmd = new NpgsqlCommand(strSql, sqlConn))
            {
                int result = 0;
                try
                {
                    result = sqlCmd.ExecuteNonQuery();
                    return result;
                }
                catch(Exception ex)
                {
                    //throw new Exception("SQL执行异常！" + ex.Message);
                    try
                    {
                        Destroy();
                        getConn();
                        using (NpgsqlCommand sqlCmd1 = new NpgsqlCommand(strSql, sqlConn))
                        {
                            result = sqlCmd.ExecuteNonQuery();
                            return result;
                        }
                    }
                    catch (Exception ex1)
                    {
                        throw new Exception("SQL执行异常！" + ex1.Message);
                    }
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
            using (NpgsqlCommand cmd = sqlConn.CreateCommand())
            {
                try
                {
                    Hashtable ht = new Hashtable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = funName;

                    foreach (var pgParameter in DBParameters)
                    {
                        if (pgParameter.parameterDirection == ParameterDirection.Input)
                        {
                            if (pgParameter.ValueType == valueTypes.STRING)
                            {
                                cmd.Parameters.Add(new NpgsqlParameter(pgParameter.ParameterName, DbType.String)).Direction = ParameterDirection.Input;
                            }
                            else if (pgParameter.ValueType == valueTypes.INT)
                            {
                                cmd.Parameters.Add(new NpgsqlParameter(pgParameter.ParameterName, DbType.Int32)).Direction = ParameterDirection.Input;
                            }
                            else
                            {
                                cmd.Parameters.Add(new NpgsqlParameter(pgParameter.ParameterName, DbType.String)).Direction = ParameterDirection.Input;
                            }
                            cmd.Parameters[pgParameter.ParameterName].Value = pgParameter.ParameterValue;
                        }
                        else if (pgParameter.parameterDirection == ParameterDirection.InputOutput)
                        {
                            if (pgParameter.ValueType == valueTypes.STRING)
                            {
                                cmd.Parameters.Add(new NpgsqlParameter(pgParameter.ParameterName, DbType.String)).Direction = ParameterDirection.InputOutput;
                            }
                            else if (pgParameter.ValueType == valueTypes.INT)
                            {
                                cmd.Parameters.Add(new NpgsqlParameter(pgParameter.ParameterName, DbType.Int32)).Direction = ParameterDirection.Input;
                            }
                            else
                            {
                                cmd.Parameters.Add(new NpgsqlParameter(pgParameter.ParameterName, DbType.String)).Direction = ParameterDirection.Input;
                            }
                            cmd.Parameters[pgParameter.ParameterName].Value = pgParameter.ParameterValue;

                        }
                        else
                        {
                            cmd.Parameters.Add(new NpgsqlParameter(pgParameter.ParameterName, DbType.String)).Direction = ParameterDirection.Output;

                        }
                    }
                    cmd.ExecuteNonQuery();//执行存储过程

                    foreach (var Parameter in DBParameters)
                    {
                        if (Parameter.parameterDirection == ParameterDirection.Input)
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


    }
}
