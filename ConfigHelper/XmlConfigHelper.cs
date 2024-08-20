using System;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConfigHelper
{
    /// <summary>
    /// 提供通用的对XML序列化对象的读写方法
    /// </summary>
    public class XmlConfigHelper
    {
        /// <summary>
        /// 获取配置文件绝对路径（当前路径为当前应用的安装目录）
        /// </summary>
        /// <param name="path">相对路径表示的文件名</param>
        /// <returns>文件绝对路径</returns>
        public string getConfigFilePath(string path)
        {
            if (File.Exists(path)) return path;
            if (!Path.IsPathRooted(path)) path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            if (File.Exists(path)) return path;
            throw new Exception("Config File Not Found!\n\r");
        }

        /// <summary>
        /// 获取配置目录绝对路径（当前路径为当前应用的安装目录）
        /// </summary>
        /// <param name="path">相对路径表示的目录名</param>
        /// <returns>目录的绝对路径</returns>
        public string getConfigPath(string path)
        {
            if (Directory.Exists(path)) return path;
            if (!Path.IsPathRooted(path)) path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            if (Directory.Exists(path)) return path;
            throw new Exception("Config Directory Not Found!\n\r");
        }

        /// <summary>
        /// 保存XML序列化对象
        /// </summary>
        /// <param name="data">xml序列化对象实例</param>
        /// <param name="type">xml序列化对象类型</param>
        /// <param name="configFilePath">xml序列化文件保存路径</param>
        public void Save(Object data, Type type, String configFilePath)
        {
            FileStream fs = null;
            XmlWriter wr = null;
            XmlSerializer sr;
            try
            {
                if (Path.GetDirectoryName(configFilePath).Length > 0 && configFilePath.IndexOfAny(new char[] { '/', '\\' }) > 0)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(configFilePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
                    }
                }

                fs = new FileStream(configFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                wr = new XmlTextWriter(fs, Encoding.UTF8);
                sr = new XmlSerializer(type);
                sr.Serialize(wr, data);
            }
            finally
            {
                if (wr != null) wr.Close();
                if (fs != null) fs.Close();
            }

        }

        /// <summary>
        /// 按二进制格式序列化对象
        /// </summary>
        /// <param name="ds">对象实例</param>
        /// <param name="configFilePath">序列化文件保存路径</param>
        /// <returns></returns>
        public bool Save(Object ds, string configFilePath)
        {
            MemoryStream memoryStream;
            IFormatter bf;
            try
            {
                if (Path.GetDirectoryName(configFilePath).Length > 0 && configFilePath.IndexOfAny(new char[] { '/', '\\' }) > 0)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(configFilePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
                    }
                }
                memoryStream = new MemoryStream();
                bf = new BinaryFormatter();
                bf.Serialize(memoryStream, ds);
                FileStream outStream = File.OpenWrite(configFilePath);
                memoryStream.WriteTo(outStream);
                outStream.Flush();
                outStream.Close();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 反序列化以xml格式序列化到文件的对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="configFilePath">序列化文件名</param>
        /// <returns>xml对象实例</returns>
        public Object Get(Type type, String configFilePath)
        {
            Object data = null;
            FileStream fs = null;
            XmlSerializer sr = null;
            try
            {
                fs = new FileStream(this.getConfigFilePath(configFilePath), FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new XmlSerializer(type);
                data = sr.Deserialize(fs);
                fs.Close();
            }
            catch { }
            finally
            {
                if (fs != null) fs.Close();
            }
            return data;
        }

        /// <summary>
        /// 反序列化二进制格式序列化到文件的对象
        /// </summary>
        /// <param name="configFilePath">文件路径</param>
        /// <returns>对象实例</returns>
        public Object Get(String configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                return null;
            }
            FileStream inStream = new FileStream(this.getConfigFilePath(configFilePath), FileMode.Open, FileAccess.Read, FileShare.Read);
            IFormatter iFormatter = new BinaryFormatter();
            Object data = iFormatter.Deserialize(inStream);
            inStream.Flush();
            inStream.Close();

            return data;
        }
    }
}
