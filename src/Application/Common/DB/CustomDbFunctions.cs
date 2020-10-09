using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.Common.DB
{
    public static class CustomDbFunctions
    {

        [DbFunction("calculateDistance")]
        public static double CalculateDistance(
            double aLat, double aLon, double bLat, double bLon)
        {
            throw new NotImplementedException();
        }

        [DbFunction("isYear")]
        public static bool IsYear(DateTime? date, int? year)
        {
            throw new NotImplementedException();
        }

        [DbFunction("isMonth")]
        public static bool IsMonth(DateTime? date, DateTime? month)
        {
            throw new NotImplementedException();
        }

        [DbFunction("isEqualDate")]
        public static bool IsEqualDate(DateTime? date1, DateTime? date2)
        {
            throw new NotImplementedException();
        }

        [DbFunction("extractYear")]
        public static int ExtractYear(DateTime? date)
        {
            throw new NotImplementedException();
        }

        [DbFunction("extractMonth")] 
        public static int ExtractMonth(DateTime? date)
        {
            throw new NotImplementedException();
        }

        [DbFunction("extractDay")]
        public static int ExtractDay(DateTime? date)
        {
            throw new NotImplementedException();
        }

    }
}
