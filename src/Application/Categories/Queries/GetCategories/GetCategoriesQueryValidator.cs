using FluentValidation;

namespace rentasgt.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
    {
        public GetCategoriesQueryValidator()
        {
            RuleFor(r => r.PageNumber)
                .GreaterThan(0).WithMessage("El numero de pagina debe ser un entero positivo");
            RuleFor(r => r.PageSize)
                .InclusiveBetween(5, 100).WithMessage("El tamaño de la pagina debe ser entre 5 y 100");
        }
    }
}
