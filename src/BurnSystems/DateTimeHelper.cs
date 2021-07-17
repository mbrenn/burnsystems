using System;

namespace BurnSystems
{
    /// <summary>
    /// This static helper class is used to manipulate DateTime-Structures
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Gets the first occurance of the hour in the past. 
        /// If current Date is 02.01. 12:00 and <c>hour</c> is 13, 
        /// the date 01.01. 13:00 will be returned
        /// </summary>
        /// <param name="time">Time to be resumed</param>
        /// <param name="hour">Requested hour</param>
        /// <returns>Time in the past with matching hour</returns>
        public static DateTime GoBackToHour(this DateTime time, int hour)
        {
            if (time.Hour >= hour)
            {
                return new DateTime(
                    time.Year,
                    time.Month,
                    time.Day,
                    hour,
                    0,
                    0);
            }

            var yesterday = time.Subtract(
                TimeSpan.FromDays(1));

            return new DateTime(
                yesterday.Year,
                yesterday.Month,
                yesterday.Day,
                hour,
                0,
                0);
        }
        
        
        /// <summary>
        /// Truncates the given date time to the given timespan
        /// </summary>
        /// <param name="dateTime">The value to be truncated</param>
        /// <param name="timeSpan">The resolution for truncation</param>
        /// <returns>The truncated date time</returns>
        public static DateTime Truncate(DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) return dateTime; // do not modify "guard" values
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static DateTime TruncateToSecond(DateTime dateTime)
        {
            return Truncate(dateTime, TimeSpan.FromSeconds(1));
        }

        public static DateTime TruncateToMinute(DateTime dateTime)
        {
            return Truncate(dateTime, TimeSpan.FromMinutes(1));
        }
    }
}
