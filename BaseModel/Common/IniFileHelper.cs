using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace BaseModel
{
    /// <summary>
    /// 此类中的方法均为静态方法
    /// </summary>
    public class IniFileHelper
    {
        #region API函数声明
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);
        //[DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        //private static extern long GetPrivateProfileString(string section, string key,
        //    string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key,
            string def, Byte[] retVal, int size, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);
        #endregion

        #region ReadSections读取指定文件所有Section字符串名
        /// <summary>
        /// 读取指定文件所有Section字符串名
        /// </summary>
        /// <param name="iniFilename">指定文件路径及文件名</param>
        /// <returns>Section字符串名列表</returns>
        public static List<string> ReadSections(string iniFilename)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(null, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            }
            return result;
        }
        #endregion

        #region ReadSectionKeys读取指定文件及Section名下的所有键值
        /// <summary>
        /// 读取指定文件及Section名下的所有键值
        /// </summary>
        /// <param name="iniFilename">指定文件路径及文件名</param>
        /// <param name="section">指定的Section名</param>
        /// <returns>键值字符串名列表</returns>
        public static List<string> ReadSectionKeys(string iniFilename, string section)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(section, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            }
            return result;
        }
        #endregion

        #region 读Ini文件
        /// <summary>
        /// 读Ini文件
        /// </summary>
        /// <param name="Section">段落</param>
        /// <param name="Key">键</param>
        /// <param name="NoText">缺省值,可传空</param>
        /// <param name="iniFilePath">Ini文件路径</param>
        /// <returns></returns>
        public static string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion

        #region 写Ini文件
        public static bool WriteIniData(string Section, string Key, string Value, string iniFilePath)
        {
            if (!File.Exists(iniFilePath))
            {
                try
                {
                    File.Create(iniFilePath);
                }
                catch (Exception ex)
                {
                    throw new Exception("文件写入出错！");
                }
            }
            long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
            if (OpStation == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
