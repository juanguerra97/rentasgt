using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace rentasgt.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Display(Name = "Foto de perfil")]
            public IFormFile ProfilePicture { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio")]
            [Display(Name = "Nombre")]
            public string FirstName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido es obligatorio")]
            [Display(Name = "Apellido")]
            public string LastName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "El correo es obligatorio")]
            [EmailAddress(ErrorMessage = "Dirección de correo inválida")]
            [Display(Name = "Correo")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Debes proveer una contraseña")]
            [StringLength(100, ErrorMessage = "La {0} debe tener entre {2} y {1} caracteres de longitud", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Repite la contraseña")]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var u = await this._userManager.FindByEmailAsync(Input.Email);
                if (u != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario con el correo ingresado");
                    return Page();
                }
                var user = new AppUser 
                { 
                    ProfileStatus = UserProfileStatus.Incomplete,
                    FirstName = Input.FirstName, LastName = Input.LastName,
                    UserName = Input.Email, Email = Input.Email,
                    ValidatedDpi = false,
                    ValidatedAddress = false 
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirma tu correo",
                        $"<div style=\"padding: 4rem 0; margin: 0; color:#212529; background-color: #f2f7fb; box-sizing: border-box;\"><div style=\"padding: 1.25rem; width: 350px; max-width: 450px; min-width: 300px; margin: 0 auto; background-color: #fff; border: none; box-shadow: 0 0 5px 0 rgba(43, 43, 43, .1), 0 11px 6px -7px rgba(43, 43, 43, .1);\"><div style=\"height: 64px;padding: 5px;border-bottom: #333 solid 1px; display: flex; align-items: center; gap: 3px;\"><img src=\"https://rentasguatemala.com/assets/tag.png\" style=\"height: 24px; margin: auto 0;\"><h1 style=\"font-size: 1.4rem;color: #FF8D27; font-family: 'Barlow Condensed', sans-serif; font-weight: 700;\">Rentas Guatemala</h1></div><div style=\"font-family: sans-serif;\"><h2 style=\"margin: 1rem 0; font-weight: normal;\">&iexcl;Hola <span style=\"font-weight: bold;\">{user.FirstName}</span>!</h2><p style=\"font-size: 1.1rem; margin: 0; padding: 0; text-align: justify;\">Bienvenid@ a Rentas Guatemala.</p><p style=\"margin: 0; padding: 0; margin-top: 0.25rem; text-align: justify;\">Para confirmar que este es tu correo, pulsa en el siguiente bot&oacute;n</p><a href=\"{HtmlEncoder.Default.Encode(callbackUrl)}\" style=\"font-size: 16px;color: #555; margin-top: 0.8rem;box-shadow: 0 1px 2px 1px #ddd;padding: 10px 19px;cursor:pointer;border-radius: 2px;text-decoration: none;display: block; text-align: center;background-color: #FBAB7E;background-image: linear-gradient(62deg, #FBAB7E 0%, #F7CE68 100%);\">CONFIRMAR</a></div></div></div>"
                        //$"<div style='padding: 0.5rem'><h1><span style='font-weight: normal; font-size: 1.2rem;'>Bienvenido a </span>Rentas Guatemala</h1><p>Para confirmar tu correo haz click en <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='font-weight: bold;'>CONFIRMAR</a></p></div>"
                        );

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
