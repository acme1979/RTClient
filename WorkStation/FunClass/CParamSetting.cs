using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WorkStation
{
    [Serializable]
    public class CParamSetting
    {
        /// <summary>
        /// 工序代码
        /// </summary>
        public string Process { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string WorkStation { get; set; }
        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkStationName { get; set; }
        /// <summary>
        /// 线体代码
        /// </summary>
        public string ProductLine { get; set; }
        /// <summary>
        /// 线体名称
        /// </summary>
        public string ProductLineName { get; set; }
        public string PrintType { get; set; }
        public string PrintDriver { get; set; }
        public string LabelTemplet { get; set; }
        public string PrintDriverName { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string DATA_AUTH { get; set; }
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string DATA_AUTHNAME { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBCode { get; set; }
        /// <summary>
        /// 工号代码
        /// </summary>
        public string EmpCode { get; set; }
        /// <summary>
        /// 工号名称
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// 厂商代码
        /// </summary>
        public string FactoryCode { get; set; }

        public void Serializer(CParamSetting instance)
        {
            string fileName = Directory.GetCurrentDirectory() + "\\Config\\WorkStationParamSetting.xml";
            Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            XmlSerializer xmlFormat = new XmlSerializer(typeof(CParamSetting), new Type[] { typeof(CParamSetting) });//创建XML序列化器，需要指定对象的类型
            xmlFormat.Serialize(fStream, instance);
            fStream.Close();
        }
        public CParamSetting Deserializer()
        {
            string fileName = Directory.GetCurrentDirectory() + "\\Config\\WorkStationParamSetting.xml";
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            XmlSerializer xmlSearializer = new XmlSerializer(typeof(CParamSetting));
            CParamSetting cps = (CParamSetting)xmlSearializer.Deserialize(fs);
            fs.Close();
            this.Process = cps.Process;
            this.ProcessName = cps.ProcessName;
            this.WorkStation = cps.WorkStation;
            this.WorkStationName = cps.WorkStationName;
            this.PrintDriver = cps.PrintDriver;
            this.PrintType = cps.PrintType;
            this.LabelTemplet = cps.LabelTemplet;
            this.ProductLine = cps.ProductLine;
            this.ProductLineName = cps.ProductLineName;
            this.DBCode = cps.DBCode;
            this.DATA_AUTH = cps.DATA_AUTH;
            this.DATA_AUTHNAME = cps.DATA_AUTHNAME;
            this.EmpCode = cps.EmpCode;
            this.EmpName = cps.EmpName;
            this.FactoryCode = cps.FactoryCode;
            
            return cps;
        }
    }
}
