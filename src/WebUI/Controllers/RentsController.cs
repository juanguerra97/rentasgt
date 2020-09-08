using Application.Rents.Queries.GetRentsOfRequestor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Common.Models;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using rentasgt.Application.Rents.Commands.CancelRent;
using rentasgt.Application.Rents.Commands.EndRent;
using rentasgt.Application.Rents.Commands.StartRent;
using rentasgt.Application.Rents.Queries.GetRentById;
using rentasgt.Application.Rents.Queries.GetRentsOfRequestor;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class RentsController : ApiController
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<RentDto>> GetById(long id)
        {
            return await Mediator.Send(new GetRentByIdQuery { RentId = id });
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedListResponse<RentRequestRentDto>>> GetOfRequestor(int pageSize = 10, int pageNumber = 1)
        {
            return await Mediator.Send(new GetRentsOfRequestorQuery { 
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        [HttpPut("{id}/start")]
        public async Task<ActionResult> StartRent(long id)
        {
            await Mediator.Send(new StartRentCommand { RentId = id });
            return NoContent();
        }

        [HttpPut("{id}/end")]
        public async Task<ActionResult> EndRent(long id)
        {
            await Mediator.Send(new EndRentCommand { RentId = id });
            return NoContent();
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelRent(long id)
        {
            await Mediator.Send(new CancelRentCommand { RentId = id });
            return NoContent();
        }

    }
}
