using System;

namespace Application.Common.Extensions
{
    public static class RentasGtDateTimeExtensions
    {

        // public static readonly TimeZoneInfo CENTRAL_AMERICA_STANDARD_TIME = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"); // WINDOWS SERVER
        public static readonly TimeZoneInfo CENTRAL_AMERICA_STANDARD_TIME = TimeZoneInfo.FindSystemTimeZoneById("America/Guatemala"); // LINUX SERVER

        public static DateTime? ToCentralAmericaStandardTime(this DateTime? fromDate) {
            if (fromDate == null) return null;
            return TimeZoneInfo.ConvertTime((DateTime)fromDate, TimeZoneInfo.Local, CENTRAL_AMERICA_STANDARD_TIME);
        }

        public static DateTime ToCentralAmericaStandardTime(this DateTime fromDate) {
            return TimeZoneInfo.ConvertTime(fromDate, TimeZoneInfo.Local, CENTRAL_AMERICA_STANDARD_TIME);
        }
        
    }
}