using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Categories.Queries.GetCategories
{
    public class CategoryDto : IMapFrom<Category>
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
