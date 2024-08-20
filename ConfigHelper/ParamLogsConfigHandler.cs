using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Collections;

namespace ConfigHelper
{
    public class ParamLogsConfigHandler
    {
        private ParamLogSettings settings;
        /// <summary>
        /// 获取参数设置类,并将其加入到内容中,不同的类需要重新定义
        /// </summary>
        public ParamLogsConfigHandler()
        {
            this.settings = ConfigurationManager.GetSection("ParamLogConfig") as ParamLogSettings;
            CacheManager appCache = CacheFactory.GetCacheManager();
            if (appCache["ParamLogs"] == null)
            {
                appCache.Add("ParamLogs", settings);
            }
        }
    }
}
