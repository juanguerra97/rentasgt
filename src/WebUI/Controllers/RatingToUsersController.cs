using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.RatingToUsers.Commands.RateUser;
using rentasgt.Application.RatingToUsers.Queries.GetPendingRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class RatingToUsersController : ApiController
    {

        [HttpGet("pending")]
        public async Task<ActionResult<RatingToUserDto>> GetPendingRating()
        {
            return await Mediator.Send(new GetPendingRatingQuery());
        }

        [HttpPost("rate")]
        public async Task<ActionResult<double>> RateUser(RateUserCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}
