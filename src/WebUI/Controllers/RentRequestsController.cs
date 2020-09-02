using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Common.Models;
using rentasgt.Application.RentRequests.Commands.AcceptRentRequest;
using rentasgt.Application.RentRequests.Commands.CancelRentRequest;
using rentasgt.Application.RentRequests.Commands.CreateRentRequest;
using rentasgt.Application.RentRequests.Commands.RejectRentRequest;
using rentasgt.Application.RentRequests.Commands.ViewRentRequest;
using rentasgt.Application.RentRequests.Queries.GetAllRentRequests;
using rentasgt.Application.RentRequests.Queries.GetAllRentRequestsOfRequestor;
using rentasgt.Application.RentRequests.Queries.GetPendingRentRequests;
using rentasgt.Application.RentRequests.Queries.GetRentRequests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class RentRequestsController : ApiController
    {

        [HttpPost("{id}/view")]
        public async Task<ActionResult> View(long id)
        {
            await Mediator.Send(new ViewRentRequestCommand { RentRequestId = id });
            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> Cancel(long id)
        {
            await Mediator.Send(new CancelRentRequestCommand { RentRequestId = id });
            return NoContent();
        }

        [HttpPost("{id}/reject")]
        public async Task<ActionResult> Reject(long id, RejectRentRequestCommand command)
        {

            if (id != command.RentRequestId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("{id}/accept")]
        public async Task<ActionResult> Accept(long id)
        {
            await Mediator.Send(new AcceptRentRequestCommand { RentRequestId = id });
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<RentRequestDto>>> GetAll(int pageSize = 10, int pageNumber = 1)
        {
            return await Mediator.Send(new GetAllRentRequestsQuery { PageSize = pageSize, PageNumber = pageNumber });
        }

        [HttpGet("pending")]
        public async Task<ActionResult<List<RentRequestDto>>> GetPending(int pageSize = 10, int pageNumber = 1)
        {
            return await Mediator.Send(new GetPendingRentRequestsQuery { PageSize = pageSize, PageNumber = pageNumber });
        }

        [HttpGet("requestor")]
        public async Task<ActionResult<PaginatedListResponse<RentRequestDto>>> GetOfRequestor(int pageSize = 10, int pageNumber = 1)
        {
            return await Mediator.Send(new GetAllRentRequestsOfRequestorQuery { PageSize = pageSize, PageNumber = pageNumber });
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateRentRequestCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}
