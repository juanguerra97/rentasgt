using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequests
{
    public class RentRequestProductOwnerDto : IMapFrom<AppUser>
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
