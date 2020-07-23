using Geolocation;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;

namespace rentasgt.Infrastructure.Services
{
    public class LocationService : ILocation
    {
        public Address GetAddress(Coordinate coordinate)
        {
            return new Address
            {
                Country = "Guatemala",
                State = "Guatemala",
                City = "Guatemala"
            };
        }
    }
}
