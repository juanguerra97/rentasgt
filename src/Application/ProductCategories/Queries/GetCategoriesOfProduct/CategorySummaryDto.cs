using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.ProductCategories.Queries.GetCategoriesOfProduct
{
    public class CategorySummaryDto : IMapFrom<Category>
    {

        public long Id { get; set; }
        public string Name { get; set; }

    }
}
