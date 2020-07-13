
using rentasgt.Application.Common.Mappings;
using rentasgt.Domain.Entities;

namespace rentasgt.Application.Products.Commands.CreateProduct
{
    public class RentCostCommand : IMapFrom<RentCost>
    {

        public int Duration { get; set; }
        public decimal Cost { get; set; }

    }
}
