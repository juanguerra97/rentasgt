using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class ProductCategoryDto : IMapFrom<ProductCategory>
    {

        public long CategoryId { get; set; }
        public string CategoryName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));
        }

    }
}
