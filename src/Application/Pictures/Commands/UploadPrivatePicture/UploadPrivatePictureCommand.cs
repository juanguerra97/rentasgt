using MediatR;
using Microsoft.AspNetCore.Http;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Pictures.Commands.UploadPrivatePicture
{

    public class UploadPrivatePictureCommand : IRequest<long>
    {

        public IFormFile ImageFile { get; set; }

    }

    public class UploadPrivatePictureCommandHandler : IRequestHandler<UploadPrivatePictureCommand, long>
    {

        private readonly IApplicationDbContext context;

        public UploadPrivatePictureCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<long> Handle(UploadPrivatePictureCommand request, CancellationToken cancellationToken)
        {

            byte[] imgBytes = await GetImageBytesAsync(request.ImageFile);
            string contentType = request.ImageFile.ContentType;
            string imgBase64String = Convert.ToBase64String(imgBytes);
            string imgContent = $"data:{contentType};base64," + imgBase64String;

            var pic = new Picture(PictureStorageType.Base64, imgContent);
            await context.Pictures.AddAsync(pic);
            await context.SaveChangesAsync(cancellationToken);

            return pic.Id;
        }

        public async Task<byte[]> GetImageBytesAsync(IFormFile imageFile)
        {
            using(var ms = new MemoryStream())
            {
                await imageFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

    }
}
