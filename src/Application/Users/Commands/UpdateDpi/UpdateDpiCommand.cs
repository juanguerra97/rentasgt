using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Users.Commands.UpdateDpi
{
    public class UpdateDpiCommand : IRequest
    {

        public string UserId { get; set; }
        public string? Cui { get; set; }
        public IFormFile? DpiPictureFile { get; set; }
        public IFormFile? UserPictureFile { get; set; }

    }

    public class UpdateDpiCommandHandler : IRequestHandler<UpdateDpiCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public UpdateDpiCommandHandler(IApplicationDbContext context, UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateDpiCommand request, CancellationToken cancellationToken)
        {
            var user = await this.context.AppUsers
                .Include(u => u.DpiPicture.Picture)
                .Include(u => u.UserPicture.Picture)
                .Include(u => u.AddressPicture.Picture)
                .Include(u => u.ProfilePicture.Picture)
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            if (this.currentUserService.UserId != request.UserId 
                && !(await this.userManager.IsInRoleAsync(user, "Moderador")))
            {
                throw new OperationForbidenException();
            }

            if (user.ProfileStatus != UserProfileStatus.Incomplete)
            {
                throw new InvalidStateException("Ya no se puede actualizar el DPI");
            }

            if (request.DpiPictureFile != null)
            {
                if (user.DpiPicture != null)
                {
                    this.context.DpiPictures.Remove(user.DpiPicture);
                }

                var dpiPic = await GetBase64(request.DpiPictureFile);
                user.DpiPicture = new DpiPicture
                {
                    Picture = new Picture
                    {
                        StorageType = PictureStorageType.Base64,
                        PictureContent = dpiPic
                    }
                };
                user.ValidatedDpi = false;
            }

            if (request.UserPictureFile != null) {
                if (user.UserPicture != null){
                    this.context.UserPictures.Remove(user.UserPicture);
                }
                var userPic = await GetBase64(request.UserPictureFile);
                user.UserPicture = new UserPicture 
                {
                    Picture = new Picture 
                    {
                        StorageType = PictureStorageType.Base64,
                        PictureContent = userPic
                    }
                };
            }

            if (user.UserPicture == null || user.AddressPicture == null || user.ProfilePicture == null)
            {
                user.ProfileStatus = UserProfileStatus.Incomplete;
            } else if (!user.ValidatedAddress || !user.ValidatedDpi)
            {
                user.ProfileStatus = UserProfileStatus.WaitingForApproval;
            }

            if (request.Cui != null)
            {
                user.Cui = request.Cui;
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        public async Task<string> GetBase64(IFormFile file)
        {
            byte[] imgBytes = await GetImageBytesAsync(file);
            string contentType = file.ContentType;
            string imgBase64String = Convert.ToBase64String(imgBytes);
            return $"data:{contentType};base64," + imgBase64String;
        }

        public async Task<byte[]> GetImageBytesAsync(IFormFile imageFile)
        {
            using (var ms = new MemoryStream())
            {
                await imageFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

    }

}
