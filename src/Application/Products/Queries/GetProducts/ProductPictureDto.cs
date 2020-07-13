using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class ProductPictureDto : IMapFrom<ProductPicture>
    {

        public long PictureId { get; set; }
        public int StorageType { get; set; }
        public string PictureContent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductPicture, ProductPictureDto>()
                .ForMember(d => d.PictureId, opt => opt.MapFrom(s => s.PictureId))
                .ForMember(d => d.StorageType, opt => opt.MapFrom(s => (int)s.Picture.StorageType))
                .ForMember(d => d.PictureContent, opt => opt.MapFrom(s => s.Picture.PictureContent));
        }

    }
}