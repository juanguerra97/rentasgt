using rentasgt.Domain.Enums;
using System;

namespace rentasgt.Domain.Entities
{
    public class RatingToProduct
    {

        public static readonly int MAX_COMMENT_LENGTH = 1024;

        public long Id { get; set; }
        public RatingStatus Status { get; set; }

        public string FromUserId { get; set; }
        public AppUser FromUser { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public int? ProductRatingValue { get; set; }
        public int? OwnerRatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime? RatingDate { get; set; }

    }
}
