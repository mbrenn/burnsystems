namespace BurnSystems
{
    using System;

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
            else
            {
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
        }
    }
}
