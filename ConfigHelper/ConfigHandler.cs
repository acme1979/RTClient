using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigHelper
{
    public class ConfigHandler : System.Configuration.IConfigurationSectionHandler
    {
        /// <summary>
        /// 用户反序列化对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ParamLogSettings));
            return serializer.Deserialize(new XmlNodeReader(section));
        }
    }
}
