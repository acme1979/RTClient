using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;

namespace ConfigHelper
{
    [XmlRoot("ParamLogConfig")]
    public class ParamLogSettings
    {
        private ParamLog[] paramLogs;
        [XmlArray("ParamLogs"), XmlArrayItem("ParamLog")]
        public ParamLog[] ParamLogs
        {
            set { paramLogs = value; }
            get { return paramLogs; }
        }

        /// <summary>
        /// 初始化当前对象的内容,保存至程序目录下Config\ParamLogs.Config文件中,
        /// 如果存在数组为空,则不保存,在保存时,如果当前对象不包含在缓存中,则加入
        /// </summary>
        public void Save()
        {
            CacheManager cache = CacheFactory.GetCacheManager();
            if (!cache.Contains("ParamLogs"))
            {
                cache.Add("ParamLogs", this);
            }
            File.Delete(System.Windows.Forms.Application.StartupPath + "\\Config\\ParamLogs.Config");
            XmlConfigHelper configHelper = new XmlConfigHelper();
            configHelper.Save(this, typeof(ParamLogSettings), System.Windows.Forms.Application.StartupPath + "\\Config\\ParamLogs.Config");
        }

        /// <summary>
        /// 获取参数日志名所对应的参数类
        /// </summary>
        /// <param name="name">参数日志名称</param>
        /// <returns>参数类</returns>
        public ParamLog GetParamLog(string name)
        {
            CacheManager cache = CacheFactory.GetCacheManager();
            ParamLogSettings settings;
            ParamLog config = null;
            //判断缓存中是否包含此参数日志名称所对应的类
            if (cache.Contains("ParamLogs"))
            {
                settings = cache.GetData("ParamLogs") as ParamLogSettings;
                for (int i = 0; i < settings.ParamLogs.Length; i++)
                {
                    if (settings.ParamLogs[i].name == name)
                    {
                        config = settings.ParamLogs[i];
                        break;
                    }
                }
            }
            //如果不包含,则config为空,为其初始化一个空类
            if (config == null)
            {
                config = new ParamLog();
            }
            //判断存放ParamLog的数组中,是否创建,没有则创建一个,并将当前对象加入到此数组中
            //如果有,则改变当前数组长度,将此对象加入
            if (paramLogs == null)
            {
                paramLogs = new ParamLog[1];
                paramLogs[0] = config;
            }
            else
            {
                bool b = false;
                for (int k = 0; k < paramLogs.Length; k++)
                { 
                    if(paramLogs[k] == config)
                    {
                        b = true;
                        break;
                    }
                }
                if(!b)
                {
                    ParamLog[] temp = paramLogs;
                    paramLogs = new ParamLog[temp.Length + 1];
                    for (int i = 0; i < paramLogs.Length - 1; i++)
                    {
                        paramLogs[i] = temp[i];
                    }
                    paramLogs[paramLogs.Length - 1] = config;
                }
            }
            return config;
        }
    }

    public class ParamLog
    {
        [XmlAttribute("name")]
        public string name;
        [XmlAttribute("Param1")]
        public string Param1;
        [XmlAttribute("Param2")]
        public string Param2;
        [XmlAttribute("Param3")]
        public string Param3;
        [XmlAttribute("Param4")]
        public string Param4;
        [XmlAttribute("Param5")]
        public string Param5;
        [XmlAttribute("Param6")]
        public string Param6;
        [XmlAttribute("Param7")]
        public string Param7;
        [XmlAttribute("Param8")]
        public string Param8;
        [XmlAttribute("Param9")]
        public string Param9;
        [XmlAttribute("Param10")]
        public string Param10;
        [XmlAttribute("Param11")]
        public string Param11;
        [XmlAttribute("Param12")]
        public string Param12;
        [XmlAttribute("Param13")]
        public string Param13;
        [XmlAttribute("Param14")]
        public string Param14;
        [XmlAttribute("Param15")]
        public string Param15;
        [XmlAttribute("Param16")]
        public string Param16;
        [XmlAttribute("Param17")]
        public string Param17;
        [XmlAttribute("Param18")]
        public string Param18;
        [XmlAttribute("Param19")]
        public string Param19;
        [XmlAttribute("Param20")]
        public string Param20;
        [XmlAttribute("Param21")]
        public string Param21;
        [XmlAttribute("Param22")]
        public string Param22;
        [XmlAttribute("Param23")]
        public string Param23;
        [XmlAttribute("Param24")]
        public string Param24;
        [XmlAttribute("Param25")]
        public string Param25;
        [XmlAttribute("Param26")]
        public string Param26;
        [XmlAttribute("Param27")]
        public string Param27;
        [XmlAttribute("Param28")]
        public string Param28;
        [XmlAttribute("Param29")]
        public string Param29;
        [XmlAttribute("Param30")]
        public string Param30;
    }
}
