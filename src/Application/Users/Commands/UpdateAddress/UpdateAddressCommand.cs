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

namespace rentasgt.Application.Users.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest
    {
        public string UserId { get; set; }
        public string? Address { get; set; }
        public IFormFile? AddressPictureFile { get; set; }
    }

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public UpdateAddressCommandHandler(IApplicationDbContext context, UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var user = await this.context.AppUsers
                .Include(u => u.AddressPicture.Picture)
                .Include(u => u.UserPicture.Picture)
                .Include(u => u.DpiPicture.Picture)
                .Include(u => u.ProfilePicture.Picture)
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            var isCurrentUser = request.UserId == this.currentUserService.UserId;
            var isModerator = !isCurrentUser && (await this.userManager.IsInRoleAsync(user, "Moderador"));

            if (!isCurrentUser && !isModerator)
            {
                throw new OperationForbidenException();
            }

            if (user.ProfileStatus != UserProfileStatus.Incomplete)
            {
                throw new InvalidStateException("Ya no se puede actualizar el DPI");
            }

            if (request.AddressPictureFile != null )
            {
                if (user.AddressPicture != null) {
                    this.context.AddressPictures.Remove(user.AddressPicture);
                } 
                var addressPic = await GetBase64(request.AddressPictureFile);
                    user.AddressPicture = new AddressPicture
                    {
                        Picture = new Picture
                        {
                            StorageType = PictureStorageType.Base64,
                            PictureContent = addressPic
                        }
                    };
                    user.ValidatedAddress = false;
                
            } else if (user.AddressPicture == null)
            {
                // handle error
            }
            if (request.Address != null)
            {
                user.Address = request.Address;
            }
            if (user.DpiPicture == null || user.UserPicture == null || user.ProfilePicture == null)
            {
                user.ProfileStatus = UserProfileStatus.Incomplete;
            }
            else if (!user.ValidatedAddress || !user.ValidatedDpi)
            {
                user.ProfileStatus = UserProfileStatus.WaitingForApproval;
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
