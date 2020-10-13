using rentasgt.Application.Common.Interfaces;
using Application.Common.Extensions;
using System;

namespace rentasgt.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now.ToCentralAmericaStandardTime();
    }
}
