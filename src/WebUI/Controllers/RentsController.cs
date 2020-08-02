using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Rents.Commands.EndRent;
using rentasgt.Application.Rents.Commands.StartRent;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class RentsController : ApiController
    {

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
            await Mediator.Send(new StartRentCommand { RentId = id });
            return NoContent();
        }

    }
}
