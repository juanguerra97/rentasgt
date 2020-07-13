using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace rentasgt.Application.Pictures.Commands.UploadPublicPicture
{
    public class UploadPublicPictureCommand : IRequest<long>
    {
        public IFormFile ImageFile { get; set; }

    }

    public class UploadPublicPictureCommandHandler : IRequestHandler<UploadPublicPictureCommand, long>
    {

        private readonly string imagesPath;
        private readonly IApplicationDbContext context;
        private readonly IHostingEnvironment environment;

        public UploadPublicPictureCommandHandler(IApplicationDbContext context, IHostingEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
            imagesPath = Path.Combine(environment.WebRootPath, "uploads", "images");
        }

        public async Task<long> Handle(UploadPublicPictureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string imageFileName = GenerateUniqueFileName(request.ImageFile);
                string imageUri = Path.Combine(imagesPath, imageFileName);
                EnsureCreateUploadedImagesDirectory();
                await SaveImageToFileSystem(request.ImageFile, imageUri);

                var pic = new Picture(PictureStorageType.Url, "/uploads/images/" + imageFileName);
                context.Pictures.Add(pic);
                await context.SaveChangesAsync(cancellationToken);
                return pic.Id;
            }
            catch (Exception ex)
            {
                throw new PictureUploadException($"Image {request.ImageFile.FileName} could not be uploaded", ex);
            }
        }

        private void EnsureCreateUploadedImagesDirectory()
        {
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
        }

        private string GenerateUniqueFileName(IFormFile file)
        {
            return Guid.NewGuid().ToString() + file.FileName;
        }

        private async Task SaveImageToFileSystem(IFormFile file, string imageUri)
        {
            using (FileStream filestream = File.Create(imageUri))
            {
                await file.CopyToAsync(filestream);
                await filestream.FlushAsync();
            }
        }

    }

}
