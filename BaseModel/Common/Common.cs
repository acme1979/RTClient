using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
using System.Reflection;

namespace BaseModel
{
    public class Common
    {
        #region public static string GetStrFromHastable(Hashtable args)
        /// <summary>
        /// 将Hastable对象中的键和值解析成字符串参数
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>
        /// 返回值格式：“KeyName1=KeyValue1;......KeyNameN=KeyValueN”
        /// </returns>
        /// <remarks>主要用于需要动态传参的情况下</remarks>
        public static string GetStrFromHashtable(Hashtable args)
        {
            string argStr = "";
            foreach (string s in args.Keys)
            {
                string v = (string)args[s];
                v = (v == null ? "%%null%%" : v);
                v = v.Replace("\\", "\\\\");
                v = v.Replace(";", "\\;");
                argStr += (argStr == "" ? "" : ";") + s + "=" + v;
            }

            return argStr;
        }
        #endregion

        #region public static Hashtable GetHashtableFromStr(string args)
        /// <summary>
        /// 将字符串参数解析成Hashtable对象
        /// </summary>
        /// <param name="args">参数格式：“KeyName1=KeyValue1;......KeyNameN=KeyValueN”</param>
        /// <returns>返回值</returns>
        /// <remarks>主要用于需要动态传参的情况下</remarks>
        public static Hashtable GetHashtableFromStr(string args)
        {
            Hashtable values = new Hashtable();
            if (args == null || args.Trim() == "") return values;
            string s1 = args.Replace("\\\\", "$$@@##%%$$");
            s1 = s1.Replace("\\;", "##@@$$%%##");
            string[] argArray = s1.Split(';');
            foreach (string s in argArray)
            {
                if (s.Trim() == "") continue;
                int pos = s.IndexOf("=");
                if (pos <= 0) continue;
                string name = s.Substring(0, pos).Trim().ToLower();
                string val = s.Substring(pos + 1);
                val = val.Replace("$$@@##%%$$", "\\");
                val = val.Replace("##@@$$%%##", ";");
                values[name] = (val == "%%null%%" ? null : val);
            }
            return values;
        }
        #endregion

        #region GetHashTableFromEnumValue(Type enumType)
        /// <summary>
        /// 从枚举类型和它的特性读出并返回一个键值对
        /// </summary>
        /// <param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        /// <returns>键值对</returns>
        public static Hashtable GetHashTableFromEnumValue(Type enumType)
        {
            Hashtable ht = new Hashtable();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute desc = (DescriptionAttribute)arr[0];
                        ht.Add(field.Name, desc.Description);
                    }
                }
            }
            return ht;
        }
        #endregion


        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// <param name="FILE_NAME">文件路径</param>
        /// <returns>文件的编码类型</returns>

        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        /// 通过给定的文件流，判断文件的编码类型
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetType(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// 判断是否是不带 BOM 的 UTF8 格式
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    }
}
