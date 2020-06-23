using rentasgt.Domain.Enums;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Cost of rent of a product in a given duration unit
    /// </summary>
    public class RentCost
    {

        public RentCost()
        { }

        public RentCost(RentDuration duration, decimal cost, Product product)
        {
            Duration = duration;
            Cost = cost;
            Product = product;
        }

        /// <summary>
        /// Duration of the rent
        /// </summary>
        public RentDuration Duration { get; set; }

        /// <summary>
        /// Cost per one duration of the rent
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Product associated with the cost per time duration
        /// </summary>
        public long ProductId { get; set; }
        public Product Product { get; set; }

    }
}
