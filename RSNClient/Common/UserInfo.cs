using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BaseModel;

namespace RTClient
{
    public class UserInfo
    {
        #region 唯一静态实例
        private UserInfo() {
            _dbRSN = DatabaseFactory.CreateDatabase("FrameWork");
            this.m_FuncDataSet.Clear();
            _dbRSN.LoadDataSet("GetSYS_FuncGroup", this.m_FuncDataSet, new string[] { "SYS_FuncGroup" }, new string[] { UserID });
            _dbRSN.LoadDataSet("GetSYS_Func", this.m_FuncDataSet, new string[] { "SYS_Func" }, new string[] { "" });
            this.m_FuncDataSet.AcceptChanges();
        }

        private static UserInfo m_Instance;
        public static UserInfo Instance
        {
            get
            {
                lock (typeof(StyleHelper))
                {
                    if (m_Instance == null)
                        m_Instance = new UserInfo();
                }
                return m_Instance;
            }
        }
        #endregion

        #region 成员变量
        private string m_UserID = "";
        private string m_UserName = ""; 
        private Database _dbRSN;
        #endregion

        #region 成员属性
        public string UserID
        {
            get{ return m_UserID; }
            set { m_UserID = value; }
        }

        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }
        #endregion
        
        #region Property FuncDataSet
        private DataSet m_FuncDataSet = new DataSet();
        /// <summary>
        /// 当前用户的所有权限信息
        /// </summary>
        public DataSet FuncDataSet
        {
            get { return this.m_FuncDataSet; }
            set
            {
                if (value != null)
                {
                    this.m_FuncDataSet.Clear();
                    _dbRSN.LoadDataSet("GetSYS_FuncGroup", this.m_FuncDataSet, new string[] { "SYS_FuncGroup" }, new string[] { UserID });
                    _dbRSN.LoadDataSet("GetSYS_Func", this.m_FuncDataSet, new string[] { "SYS_Func" }, new string[] { "" });
                    this.m_FuncDataSet.AcceptChanges();
                }
            }
        }
        #endregion

    }
}
