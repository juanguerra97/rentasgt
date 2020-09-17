using AutoMapper;
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Application.Users.Queries.GetUserProfile
{
    public class UserProfileDto : IMapFrom<AppUser>
    {

        public string Id { get; set; }
        public UserProfileStatus ProfileStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set;}
        public string PhoneNumber { get; set; }
        public string PhoneNumberConfirmed { get; set; }
        public string? Cui { get; set; }
        public bool ValidatedDpi { get; set;}
        public string? Address { get; set; }
        public bool ValidatedAddress { get; set; }
        public Picture? ProfilePicture { get; set; }
        public Picture? UserPicture { get; set; }
        public Picture? DpiPicture { get; set; }
        public Picture? AddressPicture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, UserProfileDto>()
                .ForMember(d => d.UserPicture, opt => opt.MapFrom(s => s.UserPicture.Picture))
                .ForMember(d => d.ProfilePicture, opt => opt.MapFrom(s => s.ProfilePicture.Picture))
                .ForMember(d => d.DpiPicture, opt => opt.MapFrom(s => s.DpiPicture.Picture))
                .ForMember(d => d.AddressPicture, opt => opt.MapFrom(s => s.AddressPicture.Picture));
        }
    }
}
