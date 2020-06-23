
namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Location information
    /// </summary>
    public class Ubicacion
    {

        public Ubicacion()
        { }

        public Ubicacion(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

    }
}
