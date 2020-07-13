using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class RentCostDto : IMapFrom<RentCost>
    {

        public int Duration { get; set; }
        public decimal Cost { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RentCost, RentCostDto>()
                .ForMember(d => d.Duration, opt => opt.MapFrom(s => (int)s.Duration));
        }

    }
}