using FluentValidation;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {

        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre no puede estar vacio")
                .MaximumLength(Product.MAX_NAME_LENGTH).WithMessage($"El nombre no puede sobrepasar los {Product.MAX_NAME_LENGTH} caracteres")
                .When(p => p.Name != null);

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("La descripcion no puede estar vacia")
                .MaximumLength(Product.MAX_DESCRIPTION_LENGTH).WithMessage($"La descripcion no puede sobrepasar los ${Product.MAX_DESCRIPTION_LENGTH} caracteres")
                .When(p => p.Description != null); ;

            RuleFor(p => p.OtherNames)
                .NotEmpty().WithMessage("Debe proveer al menos un nombre alternativo del artículo")
                .MaximumLength(Product.MAX_OTHERNAMES_LENGTH).WithMessage($"Los nombres alternativos no pueden sobrepasar los {Product.MAX_OTHERNAMES_LENGTH} caracteres")
                .When(p => p.OtherNames != null); ;

            RuleFor(p => p.Location.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Valor de latitud invalido")
                .When(c => c.Location != null);

            RuleFor(p => p.Location.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Valor de longitud invalido")
                .When(c => c.Location != null);

        }

    }
}
