using FluentValidation;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {

        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(Product.MAX_NAME_LENGTH).WithMessage($"El nombre no puede sobrepasar los {Product.MAX_NAME_LENGTH} caracteres");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Debes proveer una breve descripción del artículo")
                .MaximumLength(Product.MAX_DESCRIPTION_LENGTH).WithMessage($"La descripcion no puede sobrepasar los ${Product.MAX_DESCRIPTION_LENGTH} caracteres");

            RuleFor(p => p.OtherNames)
                .NotEmpty().WithMessage("Debe proveer al menos un nombre alternativo del artículo")
                .MaximumLength(Product.MAX_OTHERNAMES_LENGTH).WithMessage($"Los nombres alternativos no pueden sobrepasar los {Product.MAX_OTHERNAMES_LENGTH} caracteres");

            RuleFor(p => p.Location)
                .NotEmpty().WithMessage("La ubicación del artículo es obligatoria");

            RuleFor(p => p.Location.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Valor de latitud invalido")
                .When(c => c.Location != null);

            RuleFor(p => p.Location.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Valor de longitud invalido")
                .When(c => c.Location != null);

            RuleFor(p => p.Costs)
                .NotEmpty().WithMessage("Debes proveer al menos un costo de renta");

            RuleFor(p => p.Pictures)
                .NotEmpty().WithMessage("Debes proveer al menos una imagen del artículo");

        }

    }
}
