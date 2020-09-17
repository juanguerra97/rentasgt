using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Users.Commands.UpdateProfileInfo
{
    public class UpdateProfileInfoCommand : IRequest
    {

        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile ProfilePictureFile { get; set; }

    }

    public class UpdateProfileInfoCommandHandler : IRequestHandler<UpdateProfileInfoCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly ICurrentUserService currentUserService;

        private readonly string ProfilePicturesPath;

        public UpdateProfileInfoCommandHandler(IApplicationDbContext context, UserManager<AppUser> userManager, 
            ICurrentUserService currentUserService, IHostingEnvironment environment)
        {
            this.context = context;
            this.userManager = userManager;
            this.currentUserService = currentUserService;
            ProfilePicturesPath = Path.Combine(environment.WebRootPath, "uploads", "profile");
        }

        public async Task<Unit> Handle(UpdateProfileInfoCommand request, CancellationToken cancellationToken)
        {

            var user = await this.context.AppUsers
                .Include(u => u.ProfilePicture.Picture)
                .Include(u => u.AddressPicture.Picture)
                .Include(u => u.UserPicture.Picture)
                .Include(u => u.DpiPicture.Picture)
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

            if (request.FirstName != null)
            {
                user.FirstName = request.FirstName;
            }
            if (request.LastName != null)
            {
                user.LastName = request.LastName;
            }
            if (request.ProfilePictureFile != null)
            {
                try
                {
                    string imageFileName = GenerateUniqueFileName(request.ProfilePictureFile);
                    string imageUri = Path.Combine(ProfilePicturesPath, imageFileName);
                    EnsureCreateProfileImagesDirectory();
                    await SaveImageToFileSystem(request.ProfilePictureFile, imageUri);

                    if (user.ProfilePicture != null)
                    {
                        this.context.ProfilePictures.Remove(user.ProfilePicture);
                    }

                    user.ProfilePicture = new ProfilePicture
                    {
                        Picture = new Picture(PictureStorageType.Url, "/uploads/profile/" + imageFileName)
                    };
                }
                catch (Exception ex)
                {
                    throw new PictureUploadException($"Image {request.ProfilePictureFile.FileName} could not be uploaded", ex);
                }
            }

            if (user.DpiPicture == null || user.ProfilePicture == null || user.AddressPicture == null 
                || user.UserPicture == null)
            {
                user.ProfileStatus = UserProfileStatus.Incomplete;
            } else if(!user.ValidatedAddress || !user.ValidatedDpi) {
                user.ProfileStatus = UserProfileStatus.WaitingForApproval;
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private void EnsureCreateProfileImagesDirectory()
        {
            if (!Directory.Exists(ProfilePicturesPath))
            {
                Directory.CreateDirectory(ProfilePicturesPath);
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
