using System;
using System.Collections.Generic;
using System.Text;

namespace BaseModel
{
    public class DateTimeFormatConvert
    {

        #region 唯一实例
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private DateTimeFormatConvert() 
        {

        }

        /// <summary>
        /// 唯一静态实例
        /// </summary>
        private static DateTimeFormatConvert m_Instance = null;
        /// <summary>
        /// 获取唯一静态实例
        /// </summary>
        /// <remarks>
        /// 所有需要使用CLabBizHelper对象的功能的地方均通过该属性引用
        /// </remarks>
        public static DateTimeFormatConvert Instance
        {
            get
            {
                lock (typeof(DateTimeFormatConvert))
                {
                    if (m_Instance == null)
                        m_Instance = new DateTimeFormatConvert();
                    return m_Instance;
                }
            }
        }
        #endregion
        #region SList34 34进制字符串
        string SList34 = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        #endregion

        #region SList36 36进制字符串
        string SList36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        #endregion

        #region 依据日期代码和日期，产生对应的字符串
        /// <summary>
        /// YMD（Y年最后两位转成34进制，M月份的34进制，D天的34进制）
        /// YMDD(Y年最后一位，M月份的34进制，DD真实的天数)
        /// YMDD34(Y年最后两位转成34进制，M月份的34进制，DD真实的天数)
        /// WWLL(两位周别，两位天数) WWLL34 年的最后一位+周别，对34取整转换成34进制为十位部分，对34取余为个位，加上天数
        /// YWWD（年周周天）
        /// YWW（年周周）
        /// YYWW（年年周周）
        /// MD（客户的特殊需求，从2014年3月至2016年9月的34进制数）
        /// YYYYMMDD(年年年年月月天天)
        /// </summary>
        /// <param name="datetime">待转换日期</param>
        /// <param name="dateKind">需要转换的日期格式</param>
        /// <returns></returns>
        public string GetDateCode(DateTime datetime, string dateKind)
        {
            string dateCode = "";
            if (dateKind == "YMD")
            {
                string year = datetime.Year.ToString().Substring(2, 2);
                int month = datetime.Month;
                int day = datetime.Day;
                dateCode = SList34.Substring(Convert.ToInt16(year), 1) + SList34.Substring(month, 1) + SList34.Substring(day, 1);
            }
            if (dateKind == "YMDD")
            {
                string year = datetime.Year.ToString().Substring(3, 1);
                int month = datetime.Month;
                int day = datetime.Day;
                dateCode = year + SList34.Substring(month, 1) + day.ToString("D2");
            }
            if (dateKind == "YMDD34")
            {
                string year = datetime.Year.ToString().Substring(2, 2);
                int month = datetime.Month;
                int day = datetime.Day;
                dateCode = SList34.Substring(Convert.ToInt16(year), 1) + SList34.Substring(month, 1) + SList34.Substring(day, 1);
            }
            if (dateKind == "YMD36")
            {
                string year = datetime.Year.ToString().Substring(3, 1);
                int month = datetime.Month;
                int day = datetime.Day;
                dateCode = year + SList36.Substring(month, 1) + SList36.Substring(day, 1);
            }
            if (dateKind == "WWLL")
            {
                System.Globalization.CultureInfo gc = new System.Globalization.CultureInfo("zh-CN");
                int week = gc.Calendar.GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday);
                DayOfWeek day = datetime.DayOfWeek;
                dateCode = week.ToString("D2") + ((int)day + 1).ToString("D2");
            }
            if (dateKind == "WWLL34")
            {
                System.Globalization.CultureInfo gc = new System.Globalization.CultureInfo("zh-CN");
                int week = gc.Calendar.GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday);
                DayOfWeek day = datetime.DayOfWeek;
                string year = datetime.Year.ToString().Substring(3, 1);
                int yearWeek = Convert.ToInt32(year) * 100 + week;
                int partInt = yearWeek / 34;
                int partRem = yearWeek % 34;
                dateCode = SList34.Substring(partInt, 1) + SList34.Substring(partRem, 1) + ((int)day + 1).ToString("D2");
            }
            if (dateKind == "YWWD")
            {
                System.Globalization.CultureInfo gc = new System.Globalization.CultureInfo("zh-CN");
                int week = gc.Calendar.GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday);
                DayOfWeek day = datetime.DayOfWeek + 1;
                string year = datetime.Year.ToString();
                year = year.Substring(3, 1);
                dateCode = year + week.ToString("D2") + ((int)day).ToString();
            }
            if (dateKind == "WWD")
            {
                System.Globalization.CultureInfo gc = new System.Globalization.CultureInfo("zh-CN");
                int week = gc.Calendar.GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday);
                DayOfWeek day = datetime.DayOfWeek + 1;
                dateCode = week.ToString("D2") + ((int)day).ToString();
            }
            if (dateKind == "YWW")
            {
                System.Globalization.CultureInfo gc = new System.Globalization.CultureInfo("zh-CN");
                int week = gc.Calendar.GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday);
                string year = datetime.Year.ToString().Substring(3, 1);
                dateCode = year + week.ToString("D2");
            }
            if (dateKind == "YYWW")
            {
                System.Globalization.CultureInfo gc = new System.Globalization.CultureInfo("zh-CN");
                int week = gc.Calendar.GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday);
                string year = datetime.Year.ToString().Substring(2, 2);
                dateCode = year + week.ToString("D2");
            }
            if (dateKind == "YYYYMMDD")
            {
                dateCode = datetime.ToString("YYYYMMDD");
            }
            if (dateKind == "MD")
            {
                if (datetime > Convert.ToDateTime("2016-10-01"))
                {
                    throw new Exception("格式为MD的日期超出了最大范围(最大日期为2016-09-31)！");
                }
                int year = datetime.Year;
                int month = datetime.Month;
                int day = datetime.Day;
                switch (year)
                {
                    case 2014: dateCode = SList34.Substring(month + 1, 1); break;
                    case 2015: dateCode = SList34.Substring(month + 13, 1); break;
                    case 2016: dateCode = SList34.Substring(month + 25, 1); break;
                }
                dateCode = dateCode + SList34.Substring(day + 1, 1);
            }
            return dateCode;
        }
        #endregion
    }
}
