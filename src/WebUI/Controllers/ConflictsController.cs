using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Common.Models;
using rentasgt.Application.Conflicts.Commands.CancelConflict;
using rentasgt.Application.Conflicts.Commands.CreateConflict;
using rentasgt.Application.Conflicts.Commands.FinishConflict;
using rentasgt.Application.Conflicts.Commands.ModerateConflict;
using rentasgt.Application.Conflicts.Queries.GetConflicts;
using rentasgt.Domain.Enums;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class ConflictsController : ApiController
    {

        [Authorize(Policy = "OnlyModerador")]
        [HttpGet]
        public async Task<ActionResult<PaginatedListResponse<ConflictDto>>> GetConflicts(
            int pageSize = 15, int pageNumber = 1, int? conflictStatus = null, string? moderatorId = null)
        {
            return await Mediator.Send(new GetConflictsQuery 
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                ModeratorId = moderatorId,
                ConflictStatus = (ConflictStatus?)conflictStatus,  
            });
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateConflictCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> Cancel(long id)
        {
            await Mediator.Send(new CancelConflictCommand { ConflictId = id });
            return NoContent();
        }

        [Authorize(Policy = "OnlyModerador")]
        [HttpPut("{id}/finish")]
        public async Task<ActionResult> Finish(long id)
        {
            await Mediator.Send(new FinishConflictCommand { ConflictId = id });
            return NoContent();
        }

        [Authorize(Policy = "OnlyModerador")]
        [HttpPut("{id}/moderate")]
        public async Task<ActionResult> Moderate(long id)
        {
            await Mediator.Send(new ModerateConflictCommand { ConflictId = id });
            return NoContent();
        }

    }
}
