
namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Location information
    /// </summary>
    public class Ubicacion
    {

        public static readonly int MAX_CITY_LENGTH = 128;
        public static readonly int MAX_STATE_LENGTH = 128;
        public static readonly int MAX_STATIC_MAP_LENGTH = 4096;

        public Ubicacion()
        { }

        public Ubicacion(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? StaticMap { get; set; }

    }
}
