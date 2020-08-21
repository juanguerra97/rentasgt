using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class ProductDto : IMapFrom<Product>
    {

        public long Id { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OtherNames { get; set; }
        public ProductOwnerDto Owner { get; set; }
        public UbicacionDto Location { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal? CostPerWeek { get; set; }
        public decimal? CostPerMonth { get; set; }
        public List<ProductPictureDto> Pictures { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status));
        }

    }
}
