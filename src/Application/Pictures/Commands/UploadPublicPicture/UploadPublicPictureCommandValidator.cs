using FluentValidation;
using System.Text.RegularExpressions;

namespace rentasgt.Application.Pictures.Commands.UploadPublicPicture
{
    public class UploadPublicPictureCommandValidator : AbstractValidator<UploadPublicPictureCommand>
    {

        private readonly Regex ImageContentTypeRegex = new Regex(@"^image/(png|jpg|jpeg)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly int MaxImageSize = 5 * 1024 * 1024; // 5MB

        public UploadPublicPictureCommandValidator()
        {
            RuleFor(c => c.ImageFile)
                .NotNull().WithMessage("Falta la imagen");
            
            RuleFor(c => c.ImageFile)
                .Must(i => i.Length <= MaxImageSize).WithMessage("El tamaño de la imagen debe ser menor a 5MB")
                .Must(i => ImageContentTypeRegex.IsMatch(i.ContentType)).WithMessage("El formato de la imagen debe ser png, jpg o jpeg")
                .When(c => c.ImageFile != null); ;
        }

    }
}
