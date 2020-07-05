using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Pictures.Commands.UploadPublicPicture;
using rentasgt.Application.Pictures.Commands.UploadPrivatePicture;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class PicturesController : ApiController
    {

        [HttpPost("upload/public")]
        public async Task<ActionResult<long>> UploadPublicPicture([FromForm] UploadPublicPictureCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("upload/private")]
        public async Task<ActionResult<long>> UploadPrivatePicture([FromForm] UploadPrivatePictureCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}
