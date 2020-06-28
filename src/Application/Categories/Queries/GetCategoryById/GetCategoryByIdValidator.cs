using FluentValidation;
namespace rentasgt.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdValidator()
        {
            RuleFor(r => r.Id)
                .GreaterThan(0).WithMessage("El Id debe ser un entero positivo");
        }
    }
}
