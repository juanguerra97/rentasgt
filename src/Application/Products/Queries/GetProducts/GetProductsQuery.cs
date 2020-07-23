﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Geolocation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int Distance { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public GetProductsQuery()
        { }

    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {

            var products = this.context.Products
                .Include(p => p.Owner)
                .Include(p => p.Pictures).ThenInclude(pp => pp.Picture)
                .Include(p => p.Costs)
                .Include(p => p.Categories).ThenInclude(pc => pc.Category)
                .Where(p => p.Status != ProductStatus.Inactive && p.Status != ProductStatus.Incomplete);

            if (request.Latitude != null && request.Longitude != null)
            {
                products = products.Where(p => GeoCalculator.GetDistance(new Coordinate { Latitude = (double)request.Latitude, Longitude = (double)request.Longitude }, new Coordinate { Latitude = (double)p.Location.Latitude, Longitude = (double)p.Location.Longitude }, 2, DistanceUnit.Kilometers) <= request.Distance);
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

            return await products
                .OrderBy(p => p.Id)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ProjectTo<ProductDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }

}
