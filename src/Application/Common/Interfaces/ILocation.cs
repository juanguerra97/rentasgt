using Geolocation;
using rentasgt.Application.Common.Models;

namespace rentasgt.Application.Common.Interfaces
{
    public interface ILocation
    {

        Address GetAddress(Coordinate coordinate);

    }
}
