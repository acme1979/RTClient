using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;

namespace BaseModel
{
    public class DictionarySectionConfigHelper
    {

        private Configuration config;
        public DictionarySectionConfigHelper()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string GetValue(string sectionGroup, string sectionName, string key)
        {
            IDictionary dictoinary = ConfigurationManager.GetSection(sectionGroup + "/" + sectionName) as IDictionary;
            return dictoinary[key].ToString();
        }

        public void SetValue(string sectionGroup, string sectionName, string key, string value)
        {
            IDictionary dictoinary = ConfigurationManager.GetSection(sectionGroup + "/" + sectionName) as IDictionary;
            dictoinary[key] = value;
            config.Save(ConfigurationSaveMode.Modified, true);
        }
    }
}
