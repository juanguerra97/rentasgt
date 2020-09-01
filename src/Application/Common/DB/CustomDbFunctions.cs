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

    }
}
