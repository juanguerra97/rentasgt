using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Application.Products.Queries.GetProducts;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.Conflicts.Queries.GetConflicts
{
    public class ProductConflictDto : IMapFrom<Product>
    {

        public long Id { get; set; }
        public ProductStatus Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OtherNames { get; set; }
        public UserConflictDto Owner { get; set; }
        public UbicacionDto Location { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal? CostPerWeek { get; set; }
        public decimal? CostPerMonth { get; set; }
        public double? DistanceInKm { get; set; }
        public List<ProductPictureDto> Pictures { get; set; }

    }
}
