using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.ConflictRecords.Commands.CreateConflictRecord;
using rentasgt.Application.ConflictRecords.Queries.GetConflictRecords;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize(Policy = "OnlyModerador")]
    public class ConflictRecordsController : ApiController
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ConflictRecordDto>>> GetConflictRecords(long id)
        {
            return await Mediator.Send(new GetConflictRecordsQuery { ConflictId = id });
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateConflictRecordCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}
