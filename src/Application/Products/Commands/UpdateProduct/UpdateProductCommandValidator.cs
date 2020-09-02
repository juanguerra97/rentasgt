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

            RuleFor(p => p.CostPerDay)
                .InclusiveBetween(1, 5000).WithMessage("El costo diario debe estar entre 1 y 5000")
                .When(p => p.CostPerDay != null);

            RuleFor(p => p.CostPerDay)
                .LessThan(p => p.CostPerWeek).WithMessage("El costo diario debe ser menor al costo semanal")
                .When(p => p.CostPerDay != null && p.CostPerWeek != null);

            RuleFor(p => p.CostPerDay)
                .LessThan(p => p.CostPerMonth).WithMessage("El costo diario debe ser menor al costo mensual")
                .When(p => p.CostPerDay != null && p.CostPerMonth != null);

            RuleFor(p => p.CostPerWeek)
                .InclusiveBetween(2, 35000).WithMessage("El costo semanal debe estar entre 2 y 35000")
                .When(p => p.CostPerWeek != null);

            RuleFor(p => p.CostPerWeek)
                .LessThan(p => p.CostPerMonth).WithMessage("El costo semanal debe ser menor al costo mensual")
                .When(p => p.CostPerWeek != null && p.CostPerMonth != null);

            RuleFor(p => p.CostPerMonth)
                .InclusiveBetween(4, 155000).WithMessage("El costo mensual debe estar entre 4 y 155000")
                .When(p => p.CostPerMonth != null);


        }

    }
}
