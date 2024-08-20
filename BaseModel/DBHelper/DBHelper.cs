using System;
using System.Collections.Generic;
using System.Text;
using ConfigHelper;
using System.Windows.Forms;
using System.Collections;

namespace BaseModel
{
    public class DBHelper
    { 
        /// <summary>
        /// 用于存放当前已经创建的数据库连接
        /// </summary>
        private Hashtable HtDBCode = new Hashtable();


        #region 构造函数
        private DBHelper()
        { }
        #endregion

        #region 静态实例
        private static DBHelper m_Instance;
        public static DBHelper Instance
        {
            get
            {
                lock (typeof(DBHelper))
                {
                    if (m_Instance == null)
                        m_Instance = new DBHelper();
                }
                return m_Instance;
            }
        }
        #endregion

        #region 刷新数据库连接对象
        /// <summary>
        /// 如果传入参数DBCode=="",则销毁所有的数据库字段,否则只销毁传入的参数值
        /// </summary>
        /// <param name="DBCode"></param>
        public void FlashDBObject(string DBCode)
        {
            if (DBCode == "")
            {
                foreach (DictionaryEntry de in HtDBCode)
                {
                    IDBHelper dbHelper = de.Value as IDBHelper;
                    dbHelper.Destroy();
                    HtDBCode.Remove(de.Key);
                }
            }
            else
            {
                if (HtDBCode.ContainsKey(DBCode))
                {
                    IDBHelper dbHelper = HtDBCode[DBCode] as IDBHelper;
                    dbHelper.Destroy();
                    HtDBCode.Remove(DBCode);
                }
            }
        }
        #endregion

        #region GetSQLSERVERInstace
        /// <summary>
        /// 用于创建SQLSERVER数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDBHelper GetSQLSERVERInstace(string DBCode, string SERVER, string DBNAME, string UserID, string Password)
        {
            IDBHelper dbHelper = null;
            if (HtDBCode.ContainsKey(DBCode))
            {
                dbHelper = HtDBCode[DBCode] as IDBHelper;
            }
            else
            {
                dbHelper = new DBSQLServerHelper(SERVER, DBNAME, UserID, Password);
                dbHelper.DBCode = DBCode;
                HtDBCode.Add(DBCode, dbHelper);
            }
            return dbHelper;
        }
        #endregion

        #region GetPGSQLInstace
        /// <summary>
        /// 用于创建PGSQL数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDBHelper GetPGSQLInstace(string DBCode, string SERVER, string DBNAME, string UserID, string Password, string Port)
        {
            IDBHelper dbHelper = null;
            if (HtDBCode.ContainsKey(DBCode))
            {
                dbHelper = HtDBCode[DBCode] as IDBHelper;
            }
            else
            {
                dbHelper = new DBPGSQLHelper(SERVER, DBNAME, UserID, Password,  Port);
                dbHelper.DBCode = DBCode;
                HtDBCode.Add(DBCode, dbHelper);
            }
            return dbHelper;
        }
        #endregion

        #region GetOracleSQLInstace
        /// <summary>
        /// 用于创建Oracle数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDBHelper GetOracleSQLInstace(string DBCode, string SERVER, string DBNAME, string UserID, string Password, string Port)
        {
            IDBHelper dbHelper = null;
            if (HtDBCode.ContainsKey(DBCode))
            {
                dbHelper = HtDBCode[DBCode] as IDBHelper;
            }
            else
            {
                dbHelper = new DBOracleSqlHelper(SERVER, DBNAME, UserID, Password, Port);
                dbHelper.DBCode = DBCode;
                HtDBCode.Add(DBCode, dbHelper);
            }
            return dbHelper;
        }
        #endregion

        #region GetSQLiteInstace
        /// <summary>
        /// 用于创建SQLite数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDBHelper GetSQLiteInstace(string DBCode, string DBFilePath)
        {
            IDBHelper dbHelper = null;
            if (HtDBCode.ContainsKey(DBCode))
            {
                dbHelper = HtDBCode[DBCode] as IDBHelper;
            }
            else
            {
                dbHelper = new DBSQLiteHelper(DBFilePath);
                dbHelper.DBCode = DBCode;
                HtDBCode.Add(DBCode, dbHelper);
            }
            return dbHelper;
        }
        #endregion

        #region GetSetupDB
        /// <summary>
        /// 用于创建配置文件的数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDBHelper GetSetupDB()
        {
            IDBHelper dbHelper = null;
            if (HtDBCode.ContainsKey(CConst.SetupInfoDBCode))
            {
                dbHelper = HtDBCode[CConst.SetupInfoDBCode] as IDBHelper;
            }
            else
            {
                dbHelper = new DBSQLiteHelper(CConst.SetupInfoDBPath);
                dbHelper.DBCode = CConst.SetupInfoDBCode;
                HtDBCode.Add(CConst.SetupInfoDBCode, dbHelper);
            }
            return dbHelper;
        }
        #endregion

        #region GetAccessInstace
        /// <summary>
        /// 用于创建Access数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDBHelper GetAccessInstace(string DBCode, string DBFilePath, string Password = "")
        {
            IDBHelper dbHelper = null;
            if (HtDBCode.ContainsKey(DBCode))
            {
                dbHelper = HtDBCode[DBCode] as IDBHelper;
            }
            else
            {
                dbHelper = new DBAccessHelper(DBFilePath, Password);
                dbHelper.DBCode = DBCode;
                HtDBCode.Add(DBCode, dbHelper);
            }
            return dbHelper;
        }
        #endregion

        #region GetDBInstace
        public IDBHelper GetDBInstace(string DBCode)
        {
            IDBHelper dbHelper = null;
            if (HtDBCode.ContainsKey(DBCode))
            {
                dbHelper = HtDBCode[DBCode] as IDBHelper;
            }
            else
            {
                EDBType dbTypeCode;
                try
                {
                    dbTypeCode = (EDBType)Enum.Parse(typeof(EDBType), DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DBTYPE")));
                }
                catch
                {
                    throw new Exception("数据类型解析错误,请检查数据库配置,是否正确!");
                }
                if (dbTypeCode == EDBType.SQLServer)
                {
                    string SERVER = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "SERVER"));
                    string DBNAME = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DBNAME"));
                    string UserID = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "UserID"));
                    string Password = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "Password"));
                    dbHelper = new DBSQLServerHelper(SERVER, DBNAME, UserID, Password);
                }
                else if (dbTypeCode == EDBType.PGSQL)
                {
                    string SERVER = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "SERVER"));
                    string DBNAME = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DBNAME"));
                    string UserID = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "UserID"));
                    string Password = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "Password"));
                    string PORT = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "PORT"));
                    dbHelper = new DBPGSQLHelper(SERVER, DBNAME, UserID, Password, PORT);
                }
                else if (dbTypeCode == EDBType.MySQL)
                {
                    string SERVER = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "SERVER"));
                    string DBNAME = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DBNAME"));
                    string UserID = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "UserID"));
                    string Password = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "Password"));
                    string PORT = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "PORT"));
                    dbHelper = new DBMySqlHelper(SERVER, DBNAME, UserID, Password, PORT);
                }
                else if (dbTypeCode == EDBType.ORACLE)
                {
                    string SERVER = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "SERVER"));
                    string DBNAME = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DBNAME"));
                    string UserID = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "UserID"));
                    string Password = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "Password"));
                    string PORT = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "PORT"));
                    dbHelper = new DBOracleSqlHelper(SERVER, DBNAME, UserID, Password, PORT);
                }
                else if (dbTypeCode == EDBType.ACCESS)
                {
                    string DbFilePath = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DbFilePath"));
                    dbHelper = new DBAccessHelper(DbFilePath);
                }
                else if (dbTypeCode == EDBType.SQLite)
                {
                    string DbFilePath = DESEncryption.Instance.Decrypt(IniSetupFileHelper.Instance.FindValue(DBCode, "DbFilePath"));
                    dbHelper = new DBSQLiteHelper(DbFilePath);
                }
                dbHelper.DBCode = DBCode;
                HtDBCode.Add(DBCode, dbHelper);
            }
            return dbHelper;
        }
        #endregion

        #region RemoveSetupDB
        /// <summary>
        /// 用于移除配置文件的数据库连接对象
        /// </summary>
        /// <returns></returns>
        public void RemoveSetupDB(string DBCode)
        {
            if (HtDBCode.ContainsKey(DBCode))
            {
                IDBHelper dbHelper = null;
                dbHelper = HtDBCode[DBCode] as IDBHelper;
                HtDBCode.Remove(DBCode);
                dbHelper.Destroy();
            }
        }
        #endregion
        
        #region TestConnection
        /// <summary>
        /// 用于测试数据库连接
        /// </summary>
        /// <param name="dbTypeCode">数据库类型，SQLSErver、Orcale等</param>
        /// <param name="SERVER">服务器地址及实例名</param>
        /// <param name="DBNAME">数据库名称</param>
        /// <param name="UserID">数据库登陆名</param>
        /// <param name="Password">数据库登陆密码</param>
        /// <param name="FilePath">数据库文件</param>
        /// <returns>测试成功返回连接对象</returns>
        public IDBHelper TestConnection(EDBType dbTypeCode, string SERVER, string DBNAME, string UserID, string Password, string PORT, string FilePath)
        {
            IDBHelper dbHelper = null;
            if (dbTypeCode == EDBType.SQLServer)
            {
                try
                {
                    dbHelper = new DBSQLServerHelper(SERVER, DBNAME, UserID, Password, PORT);
                }
                catch
                {
                    throw new Exception("MSSQL连接失败!");
                }
            }
            else if (dbTypeCode == EDBType.PGSQL)
            {
                try
                {
                    dbHelper = new DBPGSQLHelper(SERVER, DBNAME, UserID, Password, PORT);
                }
                catch
                {
                    throw new Exception("PGSQL连接失败!");
                }
            }
            else if (dbTypeCode == EDBType.ORACLE)
            {
                try
                {
                    dbHelper = new DBOracleSqlHelper(SERVER, DBNAME, UserID, Password, PORT);
                }
                catch
                {
                    throw new Exception("Oracle连接失败!");
                }
            }
            else if (dbTypeCode == EDBType.MySQL)
            {
                try
                {
                    dbHelper = new DBMySqlHelper(SERVER, DBNAME, UserID, Password, PORT);
                }
                catch
                {
                    throw new Exception("MySql连接失败!");
                }
            }
            else if (dbTypeCode == EDBType.SQLite)
            {
                try
                {
                    dbHelper = new DBSQLiteHelper(FilePath);
                }
                catch
                {
                    throw new Exception("SQLite连接失败!");
                }
            }
            else if (dbTypeCode == EDBType.ACCESS)
            {
                try
                {
                    dbHelper = new DBAccessHelper(FilePath, Password);
                }
                catch
                {
                    throw new Exception("Access连接失败!");
                }
            }
          
            return dbHelper;
        }
        #endregion
    }
}
