using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Application.RatingToUsers.Queries.GetPendingRating;
using rentasgt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rentasgt.Application.RatingToProducts.Queries.GetPendingRatingToProduct
{
    public class ProductRatingDto : IMapFrom<Product>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public UserRatingDto Owner { get; set; }
        public Picture Picture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductRatingDto>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(s => s.Pictures.FirstOrDefault().Picture));
        }

    }
}
