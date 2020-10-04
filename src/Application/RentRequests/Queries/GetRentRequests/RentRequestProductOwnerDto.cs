using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.RentRequests.Queries.GetRentRequests
{
    public class RentRequestProductOwnerDto : IMapFrom<AppUser>
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Picture ProfilePicture { get; set; }
        public double? Reputation { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, RentRequestProductOwnerDto>()
                .ForMember(d => d.ProfilePicture, opt => opt.MapFrom(s => s.ProfilePicture.Picture));
        }

    }
}
