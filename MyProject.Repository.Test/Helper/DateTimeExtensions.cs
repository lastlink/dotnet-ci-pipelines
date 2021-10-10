using System;

namespace MyProject.Repository.Test.Helper
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// return date with time to 0 to have date only
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime RemoveTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }
    }
}