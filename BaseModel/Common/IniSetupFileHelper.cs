using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BaseModel
{
    public class IniSetupFileHelper
    {
        #region IniFilePath 获取配置INI文件的路径及文件名
        /// <summary>
        /// 获取配置INI文件的路径及文件名
        /// </summary>
        private string inifile = Application.StartupPath + "\\Config\\ISetup.dat";
        public string IniFilePath
        {
            get
            {
                return inifile;
            }
        }
        #endregion

        #region Ini文件中所有键值对存放于此处
        private List<SetupParamContext> listSetupContext = new List<SetupParamContext>();
        /// <summary>
        /// 此列表中包含了所有的配置信息
        /// </summary>
        public List<SetupParamContext> ListSetupContext
        {
            get
            {
                return listSetupContext;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数用于对配置文件的初始化
        /// </summary>
        private IniSetupFileHelper()
        {
            List<string> sections = IniFileHelper.ReadSections(inifile);
            for (int i = 0; i < sections.Count; i++)
            {
                List<string> keys = IniFileHelper.ReadSectionKeys(inifile, sections[i]);
                foreach (string k in keys)
                {
                    SetupParamContext isc = new SetupParamContext(sections[i], k, IniFileHelper.ReadIniData(sections[i], k, "", inifile));
                    listSetupContext.Add(isc);
                }
            }
        }
        #endregion

        #region 单例
        private static IniSetupFileHelper m_Instance;
        public static IniSetupFileHelper Instance
        {
            get
            {
                lock (typeof(IniSetupFileHelper))
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new IniSetupFileHelper();
                    }
                }
                return m_Instance;
            }
        }
        #endregion

        #region FindValue
        /// <summary>
        /// 获取指定节点和键值对应的内容
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="key">键值</param>
        /// <returns>对应的内容</returns>
        public string FindValue(string section, string key)
        {
            SetupParamContext spc = listSetupContext.Find(((SetupParamContext sp) => sp.Section.Equals(section) && sp.Key.Equals(key)));
            if (spc == null)
            {
                //spc = new SetupParamContext(section, key, "");
                //listSetupContext.Add(spc);
                throw new Exception("未能找到节点：" + section + "-" + key + "对应的值！");
            }
            return spc.Value;
        }
        #endregion

        #region FindParamList
        /// <summary>
        /// 获取指定节点中所有的键值对应的SetupParamContext对象的列表
        /// </summary>
        /// <param name="section">节点</param>
        /// <returns>SetupParamContext对象的列表</returns>
        public List<SetupParamContext> FindParamList(string section)
        {
            List<SetupParamContext> spc;
            spc = listSetupContext.FindAll(((SetupParamContext sp) => sp.Section.Equals(section)));
            if (spc == null)
            {
                throw new Exception("该节点‘" + section + "’在配置信息中不存在！");
            }
            return spc;
        }
        /// <summary>
        /// 获取指定节点及键值对应的SetupParamContext对象
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="key">键值</param>
        /// <returns>SetupParamContext对象</returns>
        public SetupParamContext FindParamList(string section, string key)
        {
            SetupParamContext spc = listSetupContext.Find(((SetupParamContext sp) => sp.Section.Equals(section) && sp.Key.Equals(key)));
            if (spc == null)
            {
                spc = new SetupParamContext(section, key, "");
                spc.SetValue("");
                listSetupContext.Add(spc);
            }
            return spc;
        }
        #endregion

        #region ReadSections读取指定文件所有Section字符串名
        /// <summary>
        /// 读取指定文件所有Section字符串名
        /// </summary>
        /// <param name="iniFilename">指定文件路径及文件名</param>
        /// <returns>Section字符串名列表</returns>
        public List<string> ReadSections()
        {
            return IniFileHelper.ReadSections(inifile);
        }
        #endregion

        #region Save()
        /// <summary>
        /// 用于对设置参数的保存
        /// </summary>
        /// <returns>返回是否保存成功</returns>
        public bool Save()
        {
            foreach (SetupParamContext spc in listSetupContext)
            {
                if (spc.ModifyState)
                {
                    IniFileHelper.WriteIniData(spc.Section, spc.Key, spc.Value, inifile);
                }
            }
            return true;
        }
        #endregion
    }
}
