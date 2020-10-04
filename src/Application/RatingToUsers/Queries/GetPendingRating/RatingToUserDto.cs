using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.RatingToUsers.Queries.GetPendingRating
{
    public class RatingToUserDto : IMapFrom<RatingToUser>
    {
        public long Id { get; set; }
        public RatingStatus Status { get; set; }
        public UserRatingDto ToUser { get; set; }
    }


}
