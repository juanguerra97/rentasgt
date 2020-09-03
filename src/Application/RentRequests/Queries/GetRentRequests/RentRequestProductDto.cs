using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Application.Products.Queries.GetProducts;
using rentasgt.Domain.Entities;
using System.Linq;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequests
{
    public class RentRequestProductDto : IMapFrom<Product>
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public RentRequestProductOwnerDto Owner { get; set; } 

        public ProductPictureDto Picture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, RentRequestProductDto>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(s => s.Pictures.FirstOrDefault()));
        }


    }
}
