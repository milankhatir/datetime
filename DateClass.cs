using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SoftwareSupport.Extensions
{
    public static class DateClass
    {

        public static long toTimeStamp(this DateTime dt)
        {
            long result = 0;
            result = (long)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return result; 
        }
        public static DateTime DayStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day,0,0,0); 
        }
        public static DateTime DayEnd(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }
        /// <summary>
        /// اول هفته تاریخ جاری را محاسبه میکند
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime WeekStart(this DateTime dt)
        {
           if(dt.DayOfWeek != DayOfWeek.Saturday)
           {
                int dow = (int)dt.DayOfWeek;
             return dt.AddDays(-(1+dow));
           }
           else
           {
                return dt;
           }
        }

        /// <summary>
        /// آخر هفته تاریخ وروردی را محاسبه می کند
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <returns></returns>
        public static DateTime WeekEnd(this DateTime dt)
        {
            if (dt.DayOfWeek != DayOfWeek.Saturday)
            {
                int dow = (int)dt.DayOfWeek;
                return dt.AddDays(5 - dow);
            }
            else
            {
                return dt.AddDays(6);
            }
        }

        public static DateTime MonthStart(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            DateTime Result = dt.AddDays(-day + 1);
            return Result; 
        }
        public static DateTime MonthEnd(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            DateTime Result = dt;

            if (month <= 6)
            {
                Result = dt.AddDays((31-day));
            }
            else if(month > 6 && month <= 11)
            {
                Result = dt.AddDays((30 - day));
            } 
            else
            {
                if(pc.IsLeapYear(pc.GetYear(dt)))
                {
                    Result = dt.AddDays((30 - day));
                }
                else
                {
                    Result = dt.AddDays((29 - day));
                }
            }

            return Result;
        }
        public static DateTime YearStart(this DateTime dt)
        {
            DateTime Result = dt;
            PersianCalendar pc = new PersianCalendar();
            int days = pc.GetDayOfYear(dt);

            return Result.AddDays(1-days);
        }

        public static DateTime YearEnd(this DateTime dt)
        {
            DateTime Result = dt;
            PersianCalendar pc = new PersianCalendar();

            int days = pc.GetDayOfYear(dt);

            int TimeToAdd = 0; 

            if (pc.IsLeapYear(pc.GetYear(dt)))
            {
                TimeToAdd = 366 - days; 
            }
            else
            {
                TimeToAdd = 365 - days;
            }

            return Result.AddDays(TimeToAdd);
        }


        public static DateTime ParseCalenderPersianDate(this string date)
        {
            var dateParts = date.Split('/');

            int day = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);

            string year_Time = dateParts[2];
            int year = 1;

            int hour = 0;
            int minute = 0;
            int second = 0;

            if (year_Time.Contains(':'))
            {
                var day_TimePart = year_Time.Split(' ');
                year = int.Parse(day_TimePart[0]);

                var TimeParts = day_TimePart[1].Split(':');
                hour = int.Parse(TimeParts[0]);

                if (TimeParts[1].Contains('ب'))
                    hour += 12;

                return new DateTime(year, month, day, hour, minute, second, new PersianCalendar());
            }
            else
            {
                day = int.Parse(dateParts[2]);
                return new DateTime(year, month, day, new PersianCalendar());
            }


        }
        public static DateTime ParsePersianDate(this string date)
        {

            
            var dateParts = date.Split('/');

            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);

            string day_Time = dateParts[2];
            int day = 1;

            int hour = 0;
            int minute = 0;
            int second = 0; 
            
            if(day_Time.Contains(':'))
            {
                var day_TimePart = day_Time.Split(' '); 
                day  = int.Parse(day_TimePart[0]);

                var TimeParts = day_TimePart[1].Split(':');
                hour = int.Parse(TimeParts[0]);
                minute = int.Parse(TimeParts[1]);
                if(TimeParts.Length == 3)
                second = int.Parse(TimeParts[2]);

                return new DateTime(year, month, day, hour, minute, second, new PersianCalendar());
            }
            else
            {
                day = int.Parse(dateParts[2]);
                return new DateTime(year, month, day, new PersianCalendar());
            }

           
        }
  

        public static int WeekOfYear(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int doy= pc.GetDayOfYear(dt);

            return (doy / 7) + 1; 

        }

        public static DateTime ToPersianDate(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int min = pc.GetMinute(dt);

            return new DateTime(year, month, day, hour, min, 0);
        }
        public static string ToPersianSimpleDateString(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            return year +"/"+ month + "/"+ day;
        }
        public static string ToPersianDateTimeString(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int Minutes = pc.GetMinute(dt);
            return  (year + "/" + month.ToString("00") + "/" + day.ToString("00") + " " + hour.ToString("00") + ":" + Minutes.ToString("00"));
        }

        public static string ToPersianDateTimeFormat2(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int Minutes = pc.GetMinute(dt);
            return (year + "-" + month.ToString("00") + "-" + day.ToString("00") + " " + hour.ToString("00") + ":" + Minutes.ToString("00"));
        }

        public static DateTime ToMiladiDate(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.ToDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0);
        }
        public static DateTime ToMiladiDate(this string dt)
        {
            PersianCalendar pc = new PersianCalendar();
            PersianDateTime st = new PersianDateTime(Convert.ToDateTime(dt));
            return pc.ToDateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, 0, 0);
        }
        public static PersianDateTime ToPersianDateTime(this DateTime dateTime)
        {
            return new PersianDateTime(dateTime); 
        }
        public static PersianDateTime ToPersianDateTime(this string dateTime)
        {
            return new PersianDateTime(Convert.ToDateTime(dateTime));
        }

        public static string ToPersianSimpleDateString(this PersianDateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            return year + "/" + month + "/" + day;
        }

    }
}