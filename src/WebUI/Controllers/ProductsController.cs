using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Products.Commands.CreateProduct;
using rentasgt.Application.Products.Commands.DeleteProduct;
using rentasgt.Application.Products.Commands.UpdateProduct;
using rentasgt.Application.ProductCategories.Queries.GetCategoriesOfProduct;
using rentasgt.Application.Products.Queries.GetProductById;
using rentasgt.Application.Products.Queries.GetProducts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class ProductsController : ApiController
    {

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> Get(int pageSize = 15,
            int pageNumber = 1, double? lon = null, double? lat = null,
            int d = 10, string? city = null, string? state = null)
        {
            return await Mediator.Send(new GetProductsQuery
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Longitude = lon,
                Latitude = lat,
                Distance = d,
                City = city,
                State = state
            });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(long id)
        {
            return await Mediator.Send(new GetProductByIdQuery { Id = id });
        }

        [AllowAnonymous]
        [HttpGet("{id}/categories")]
        public async Task<ActionResult<List<CategorySummaryDto>>> GetCategories(long id)
        {
            return await Mediator.Send(new GetCategoriesOfProductQuery { ProductId = id });
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateProductCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteProductCommand { Id = id });
            return NoContent();
        }

    }
}
