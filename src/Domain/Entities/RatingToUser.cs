using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Domain.Entities
{
    public class RatingToUser
    {

        public static readonly int MAX_COMMENT_LENGTH = 1024;

        public long Id { get; set; }
        public RatingStatus Status { get; set; }
        public string FromUserId { get; set; }
        public AppUser FromUser { get; set; }

        public string ToUserId { get; set; }
        public AppUser ToUser { get; set; }
        public int? RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime? RatingDate { get; set; }

    }
}
