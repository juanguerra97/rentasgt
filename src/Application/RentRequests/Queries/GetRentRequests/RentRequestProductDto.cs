using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequests
{
    public class RentRequestProductDto : IMapFrom<Product>
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public RentRequestProductOwnerDto Owner { get; set; }

    }
}
