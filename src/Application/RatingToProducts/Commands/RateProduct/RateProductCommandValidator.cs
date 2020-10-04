using FluentValidation;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.RatingToProducts.Commands.RateProduct
{
    public class RateProductCommandValidator : AbstractValidator<RateProductCommand>
    {
        public RateProductCommandValidator()
        {
            RuleFor(c => c.OwnerRatingValue)
                .InclusiveBetween(0, 5).WithMessage("El valor debe estar entre {1} y {2}");
            RuleFor(c => c.ProductRatingValue)
                .InclusiveBetween(0, 5).WithMessage("El valor debe estar entre {1} y {2}");
            RuleFor(c => c.Comment)
                .MaximumLength(RatingToProduct.MAX_COMMENT_LENGTH).WithMessage($"El tamaño máximo es de {RatingToProduct.MAX_COMMENT_LENGTH} caracteres");
        }
    }
}