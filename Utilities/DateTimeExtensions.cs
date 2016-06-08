using System;

namespace Structura.Shared.Utilities
{
    public static class DateTimeExtensions
    {
        public static bool IsInTheFuture(this DateTime dt)
        {
            return dt > DateTime.UtcNow;
        }

        /// <summary>
        /// Returns the sunday of the week
        /// </summary>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            // start with sunday
            // sunday is day of week 7
            // monday is 1
            // tuesday is 2 etc

            Check.Require(date != DateTime.MinValue, "FirstDayOfWeek: DateTime.MinValue is invalid");
            return date.DayOfWeek != DayOfWeek.Sunday ? date.AddDays(-(int)date.DayOfWeek).Date : date.Date;
        }

        public static string ToFormattedDateAndTimeString(this DateTime dateTime)
        {
            return String.Format("{0:dddd MMMM dd yyyy} {0:hh:mm tt}", dateTime);
        }

        public static string ToFormattedTimeString(this DateTime dateTime)
        {
            return String.Format("{0:hh:mm tt}", dateTime);
        }

        public static bool HasValue(this DateTime dateTime)
        {
            return dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue;
        }

        public static bool HasNonEmptyValue(this DateTime? dateTime)
        {
            return dateTime.HasValue && dateTime.Value != DateTime.MinValue && dateTime.Value != DateTime.MaxValue;
        }

        public static DateTime ToUniversalTime(this DateTime dateTime, string localTimeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
            return dateTime - timeZone.BaseUtcOffset;

            //return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
        }

	    public static string ToIso8601(this DateTime dateTime)
	    {
		    return dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
	    }

        public static DateTime ToLocalTime(this DateTime dateTime, string localTimeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
            return dateTime + timeZone.BaseUtcOffset;
        }
    }
}