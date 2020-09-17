using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Users.Commands.UpdateAddress;
using rentasgt.Application.Users.Commands.UpdateDpi;
using rentasgt.Application.Users.Queries.GetUserProfile;
using rentasgt.Application.Users.Commands.UpdateProfileInfo;
using rentasgt.Application.Users.Commands.UpdatePhoneNumber;
using System.Threading.Tasks;
using rentasgt.Application.Users.Queries.GetPendingApprovalProfiles;
using rentasgt.Application.Common.Models;
using rentasgt.Application.Users.Commands.ApproveDpi;
using rentasgt.Application.Users.Commands.ApproveAddress;
using rentasgt.Application.Users.Commands.RejectProfile;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class UsersController : ApiController
    {

        [HttpGet("pending")]
        public async Task<ActionResult<PaginatedListResponse<UserProfileDto>>> GetPendingApprovalProfiles(int pageNumber = 1, int pageSize = 15)
        {
            return await Mediator.Send(new GetPendingApprovalProfilesQuery
            {
             PageNumber = pageNumber,
             PageSize = pageSize
            });
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile()
        {
            return await Mediator.Send(new GetUserProfileQuery());
        }

        [HttpPut("{id}/profile/dpi")]
        public async Task<ActionResult> UpdateDpi(string id, [FromForm] UpdateDpiCommand command)
        {

            if (id != command.UserId)
            {
                return BadRequest("Los id de usuario no coinciden");
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/profile/address")]
        public async Task<ActionResult> UpdateAddress(string id, [FromForm] UpdateAddressCommand command)
        {
            if (id != command.UserId)
            {
                return BadRequest("Los id de usuario no coinciden");
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/profile/info")]
        public async Task<ActionResult> UpdateProfileInfo(string id, [FromForm] UpdateProfileInfoCommand command)
        {
            if (id != command.UserId)
            {
                return BadRequest("Los id de usuario no coinciden");
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/phone")]
        public async Task<ActionResult> UpdatePhoneNumber(string id, UpdatePhoneNumberCommand command)
        {
            if (id != command.UserId)
            {
                return BadRequest("Los id de usuario no coinciden");
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [Authorize(Policy = "OnlyModerador")]
        [HttpPost("{id}/approveDpi")]
        public async Task<ActionResult> ApproveDpi(string id)
        {
            await Mediator.Send(new ApproveDpiCommand { UserId = id });
            return NoContent();
        }

        [Authorize(Policy = "OnlyModerador")]
        [HttpPost("{id}/approveAddress")]
        public async Task<ActionResult> ApproveAddress(string id)
        {
            await Mediator.Send(new ApproveAddressCommand { UserId = id });
            return NoContent();
        }

        [Authorize(Policy = "OnlyModerador")]
        [HttpPost("{id}/rejectProfile")]
        public async Task<ActionResult> RejectProfile(string id)
        {
            await Mediator.Send(new RejectProfileCommand { UserId = id });
            return NoContent();
        }
    }
}
