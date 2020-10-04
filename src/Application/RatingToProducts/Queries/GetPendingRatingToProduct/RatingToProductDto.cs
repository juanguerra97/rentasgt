using rentasgt.Application.Common.Mappings;
using rentasgt.Application.RatingToUsers.Queries.GetPendingRating;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.RatingToProducts.Queries.GetPendingRatingToProduct
{
    public class RatingToProductDto : IMapFrom<RatingToProduct>
    {
        public long Id { get; set; }
        public RatingStatus Status { get; set; }

        public string FromUserId { get; set; }
        public UserRatingDto FromUser { get; set; }

        public long ProductId { get; set; }
        public ProductRatingDto Product { get; set; }

        public int? ProductRatingValue { get; set; }
        public int? OwnerRatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime? RatingDate { get; set; }
    }
}
