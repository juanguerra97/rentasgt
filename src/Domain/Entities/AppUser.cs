using Microsoft.AspNetCore.Identity;
using rentasgt.Domain.Enums;
using System.Collections.Generic;

namespace rentasgt.Domain.Entities
{
    public class AppUser : IdentityUser
    {

        public static readonly int MAX_FIRSTNAME_LENGTH = 128;
        public static readonly int MAX_LASTNAME_LENGTH = 128;
        public static readonly int MAX_ADDRESS_LENGTH = 256;

        public AppUser()
        {
            Products = new List<Product>();
            ProfileEvents = new List<UserProfileEvent>();
            RentRequests = new List<RentRequest>();
        }

        public AppUser(UserProfileStatus status, string firstName, string lastName, 
            string email, string phoneNumber, ProfilePicture profilePicture,
            DpiPicture dpiPicture, UserPicture userPicture, AddressPicture addressPicture, 
            string? cui = null, string? address = null)
            : this()
        {
            ProfileStatus = status;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            PhoneNumber = phoneNumber;
            ProfilePicture = profilePicture;
            DpiPicture = dpiPicture;
            UserPicture = userPicture;
            AddressPicture = addressPicture;
            Cui = cui;
            Address = address;
        }

        /// <summary>
        /// User's profile status
        /// </summary>
        public UserProfileStatus ProfileStatus { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        /// Codigo unico de identificacion
        /// </summary>
        public string? Cui { get; set; }

        /// <summary>
        /// Picture of the official id document of the user
        /// </summary>
        public DpiPicture? DpiPicture { get; set; }

        public bool ValidatedDpi { get; set; }

        /// <summary>
        /// User's picture, this is used to verify against the picture in the official document and is kept private
        /// </summary>
        public UserPicture? UserPicture { get; set; }

        /// <summary>
        /// User's public profile picture
        /// </summary>
        public ProfilePicture? ProfilePicture { get; set; }

        /// <summary>
        /// User's address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Picture of a receipt or bill that demonstrates the user's address
        /// </summary>
        public AddressPicture? AddressPicture { get; set; }

        public bool ValidatedAddress { get; set; }

        public double? Reputation { get; set; }

        /// <summary>
        /// Products the user owns
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// List of rent requests the user has made
        /// </summary>
        public List<RentRequest> RentRequests { get; set; }

        /// <summary>
        /// List of events that occur to the user
        /// </summary>
        public List<UserProfileEvent> ProfileEvents { get; set; }

    }
}
