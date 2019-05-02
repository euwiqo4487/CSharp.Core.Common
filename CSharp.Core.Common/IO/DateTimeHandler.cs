using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 日期處理類別(靜態)
    /// </summary>
    public static class DateTimeHandler
    {
        static CultureInfo infoWestern = new CultureInfo("en");
        static CultureInfo infoTaiwan = new CultureInfo("zh-TW");

        static DateTimeHandler()
        {
            infoWestern.DateTimeFormat.Calendar = new GregorianCalendar();
            infoTaiwan.DateTimeFormat.Calendar = new TaiwanCalendar();
        }

        /// <summary>
        /// 轉成西元日期格式字串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="format">轉換格式</param>
        /// <returns>格式化後之西元日期字串</returns>
        public static string ToGregorian(this DateTime dt, string format = "")
        {
            return (format == "") ? dt.ToString(infoWestern) : dt.ToString(format, infoWestern);
        }

        /// <summary>
        /// 轉成西元日期
        /// </summary>
        /// <param name="dt">日期字串</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static DateTime ToGregorian(this string dt, string format)
        {
            DateTime tmp;
            if (format.ToLower().Count(x => x == 'y') == 3) //民國轉西元
            {
                int idx = format.IndexOf('y'), yLen = 3, twYear = int.Parse(dt.Substring(idx, yLen).Trim());
                var dt2 = dt.Remove(idx, yLen).Insert(idx, (twYear + 1911).ToString());
                try
                {
                    tmp = DateTime.ParseExact(dt2, format.Insert(idx, "y"), infoWestern, System.Globalization.DateTimeStyles.None);
                }
                catch
                {
                    tmp = DateTime.Parse(dt2);
                }
            }
            else
            {
                try
                {
                    tmp = DateTime.ParseExact(dt, format, infoWestern, System.Globalization.DateTimeStyles.None);
                }
                catch
                {
                    tmp = DateTime.Parse(dt);
                }
            }
            int year = infoWestern.DateTimeFormat.Calendar.GetYear(tmp);
            return new DateTime(year, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        /// <summary>
        /// 轉成民國日期格式字串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="format">轉換格式</param>
        /// <returns>格式化後之民國日期字串</returns>
        public static string ToTaiwanese(this DateTime dt, string format = "")
        {
            return (format == "") ? dt.ToString(infoTaiwan) : dt.ToString(format, infoTaiwan);
        }

        /// <summary>
        /// 轉成民國日期
        /// </summary>
        /// <param name="dt">日期字串</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static DateTime ToTaiwanese(this string dt, string format)
        {
            DateTime tmp;
            if (format.ToLower().Count(x => x == 'y') == 4) //西元轉民國
                tmp = DateTime.ParseExact(dt, format, infoWestern, System.Globalization.DateTimeStyles.None);
            else
                tmp = DateTime.ParseExact(dt, format, infoTaiwan, System.Globalization.DateTimeStyles.None);
            int twYear = infoTaiwan.DateTimeFormat.Calendar.GetYear(tmp);
            return new DateTime(twYear, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }
    }
}
