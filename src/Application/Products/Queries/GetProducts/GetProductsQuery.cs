using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.DB;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<PaginatedListResponse<ProductDto>>
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public long? Category { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int Distance { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public GetProductsQuery()
        { }

    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedListResponse<ProductDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PaginatedListResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {

            var products = this.context.Products
                .Include(p => p.Owner)
                .Include(p => p.Pictures).ThenInclude(pp => pp.Picture)
                .Include(p => p.Categories).ThenInclude(pc => pc.Category)
                .Where(p => p.Status != ProductStatus.Inactive && p.Status != ProductStatus.Incomplete);

            if (request.Category != null && request.Category > 0) 
            { 
                products = products.Where(p => p.Categories.Any(pc => pc.CategoryId == request.Category));
            }

            if (request.Name != null)
            {
                products = products.Where(p => EF.Functions.Like(p.Name.ToUpper(), $"%{request.Name.ToUpper()}%")
                    || EF.Functions.Like(p.OtherNames.ToUpper(), $"%{request.Name.ToUpper()}%"));
            }

            if (request.Latitude != null && request.Longitude != null)
            {
                double lat = (double) request.Latitude;
                double lon = (double) request.Longitude;
                var configuration = new MapperConfiguration(cfg => {

                    cfg.CreateMap<Product, ProductDto>()
                        .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
                        .ForMember(d => d.DistanceInKm, opt => opt.MapFrom(s => CustomDbFunctions.CalculateDistance(lat, lon, s.Location.Latitude, s.Location.Longitude)));

                    cfg.CreateMap<Ubicacion, UbicacionDto>();
                    cfg.CreateMap<AppUser, ProductOwnerDto>();
                    cfg.CreateMap<ProductPicture, ProductPictureDto>()
                        .ForMember(d => d.PictureId, opt => opt.MapFrom(s => s.PictureId))
                        .ForMember(d => d.StorageType, opt => opt.MapFrom(s => (int)s.Picture.StorageType))
                        .ForMember(d => d.PictureContent, opt => opt.MapFrom(s => s.Picture.PictureContent));
                });
                var productsDto = products
                    .ProjectTo<ProductDto>(configuration)
                    .Where(p => p.DistanceInKm <= request.Distance);

                return PaginatedListResponse<ProductDto>.ToPaginatedListResponse(
                    productsDto.OrderBy(p => p.DistanceInKm).ThenBy(p => p.CostPerDay).ThenBy(p => p.Id),
                    request.PageNumber, request.PageSize);
            }
            else
            {

                if (request.State != null)
                {
                    products = products.Where(p => p.Location.State == request.State);
                }

                if (request.City != null)
                {
                    products = products.Where(p => p.Location.City == request.City);
                }

            }

            return PaginatedListResponse<ProductDto>.ToPaginatedListResponse(
                products.OrderBy(p => p.Id).ProjectTo<ProductDto>(this.mapper.ConfigurationProvider),
                request.PageNumber, request.PageSize);
        }
    }

}
