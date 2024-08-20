using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BaseModel
{
    public class SystemParam
    {
        #region 属性集
        private IDBHelper setupDBHelper = DBHelper.Instance.GetSetupDB();
        public string GXSQL
        { get; set; }
        public string MachineSQL
        { get; set; }
        public string ProductLineSQL
        { get; set; }
        public string POSQL
        { get; set; }
        public string UserSQL
        { get; set; }
        public string DBCode
        { get; set; }
        #endregion

        #region 构造函数
        private SystemParam()
        {
            DataTable dt = setupDBHelper.GetDataTable("select * from SYSTEMPARAM", "SYSTEMPARAM");

            GXSQL = dt.Select("CODE = 'GXSQL'").Length > 0 ? dt.Select("CODE = 'GXSQL'")[0]["NAME"].ToString() : "";
            MachineSQL = dt.Select("CODE = 'MachineSQL'").Length > 0 ? dt.Select("CODE = 'MachineSQL'")[0]["NAME"].ToString() : "";
            ProductLineSQL = dt.Select("CODE = 'ProductLineSQL'").Length > 0 ? dt.Select("CODE = 'ProductLineSQL'")[0]["NAME"].ToString() : "";
            POSQL = dt.Select("CODE = 'POSQL'").Length > 0 ? dt.Select("CODE = 'POSQL'")[0]["NAME"].ToString() : "";
            UserSQL = dt.Select("CODE = 'UserSQL'").Length > 0 ? dt.Select("CODE = 'UserSQL'")[0]["NAME"].ToString() : "";
            DBCode = dt.Select("CODE = 'DBCode'").Length > 0 ? dt.Select("CODE = 'DBCode'")[0]["NAME"].ToString() : "";
        }
        #endregion

        #region 单例
        private static SystemParam m_Instance;
        public static SystemParam Instance
        {
            get
            {
                lock (typeof(SystemParam))
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new SystemParam();
                    }
                }
                return m_Instance;
            }
        }
        #endregion

        #region Save()
        public void Save()
        {
            SaveSystemParam("GXSQL", GXSQL);
            SaveSystemParam("MachineSQL", MachineSQL);
            SaveSystemParam("ProductLineSQL", ProductLineSQL);
            SaveSystemParam("POSQL", POSQL);
            SaveSystemParam("UserSQL", UserSQL);
            SaveSystemParam("DBCode", DBCode);
        }
        #endregion

        #region SaveSystemParam
        private void SaveSystemParam(string code, string name)
        {
            string sql = string.Format("select * from SYSTEMPARAM where code = '" + code + "'");
            DataTable dt = DBHelper.Instance.GetSetupDB().GetDataTable(sql, "SYSTEMPARAM");
            if (dt.Rows.Count == 0)
            {
                sql = string.Format("INSERT INTO SYSTEMPARAM(code,name)values('{0}','{1}')", code, name);
            }
            else
            {
                sql = string.Format("UPDATE SYSTEMPARAM set name='{1}' where code = '{0}'", code, name);
            }
            DBHelper.Instance.GetSetupDB().ExecuteSql(sql);
        }
        #endregion
    }
}
