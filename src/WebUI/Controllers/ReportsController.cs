using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Reports.Queries.RentsByDay;
using rentasgt.Application.Reports.Queries.RentsByMonth;
using rentasgt.Application.Reports.Queries.RentsByYear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    
    public class ReportsController : ApiController
    {

        [HttpGet("byday")]
        public async Task<ActionResult<RentsByDayReport>> GetRentsByDay(int year, int month)
        {
            return await Mediator.Send(new RentsByDayQuery 
            { 
                Year = year,
                Month = month
            });
        }

        [HttpGet("bymonth")]
        public async Task<ActionResult<RentsByMonthReport>> GetRentsByMonth(int year)
        {
            return await Mediator.Send(new RentsByMonthQuery { Year = year });
        }

        [HttpGet("byyear")]
        public async Task<ActionResult<RentsByYearReport>> GetRentsByYear()
        {
            return await Mediator.Send(new RentsByYearQuery());
        }

    }
}
