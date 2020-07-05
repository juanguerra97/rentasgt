using FluentValidation;
using System.Text.RegularExpressions;

namespace rentasgt.Application.Pictures.Commands.UploadPrivatePicture
{
    public class UploadPrivatePictureCommandValidator : AbstractValidator<UploadPrivatePictureCommand>
    {

        private readonly Regex ImageContentTypeRegex = new Regex(@"^image/(png|jpg|jpeg)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly int MaxImageSize = 5 * 1024 * 1024; // 5MB

        public UploadPrivatePictureCommandValidator()
        {
            RuleFor(c => c.ImageFile)
                .NotNull().WithMessage("Falta la imagen");
            RuleFor(c => c.ImageFile)
                .Must(i => i.Length <= MaxImageSize).WithMessage("El tamaño de la imagen debe ser menor a 5MB")
                .Must(i => ImageContentTypeRegex.IsMatch(i.ContentType)).WithMessage("El formato de la imagen debe ser png, jpg o jpeg")
                .When(c => c.ImageFile != null);
        }
    }
}
