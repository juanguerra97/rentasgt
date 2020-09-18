using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Products.Queries.GetProducts
{
    public class UbicacionDto : IMapFrom<Ubicacion>
    {

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public string? StaticMap { get; set; }

    }
}