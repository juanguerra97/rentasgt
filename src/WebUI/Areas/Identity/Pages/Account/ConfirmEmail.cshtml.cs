using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace rentasgt.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IApplicationDbContext context;

        public ConfirmEmailModel(UserManager<AppUser> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            this.context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"El usuario con ID '{userId}' no existe.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                var userEntity = await this.context.AppUsers
                .Include(u => u.DpiPicture.Picture)
                .Include(u => u.UserPicture.Picture)
                .Include(u => u.AddressPicture.Picture)
                .Include(u => u.ProfilePicture.Picture)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

                if (userEntity.ProfileStatus == UserProfileStatus.Incomplete
                    && (userEntity.ProfilePicture != null && userEntity.DpiPicture != null
                    && userEntity.UserPicture != null && userEntity.AddressPicture != null
                    && userEntity.PhoneNumberConfirmed))
                {
                    userEntity.ProfileStatus = UserProfileStatus.WaitingForApproval;
                    await this.context.SaveChangesAsync(CancellationToken.None);
                }
            }
            StatusMessage = result.Succeeded ? "Gracias por confirmar tu correo." : "Ocurrió un error y no pudimos confirmar tu correo.";
            return Page();
        }
    }
}
