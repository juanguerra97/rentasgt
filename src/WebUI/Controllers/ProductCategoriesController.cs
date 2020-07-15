using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.ProductCategories.Commands.CreateProductCategory;
using rentasgt.Application.ProductCategories.Commands.DeleteProductCategory;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize]
    public class ProductCategoriesController : ApiController
    {
        
        [HttpPost]
        public async Task<ActionResult> Create(CreateProductCategoryCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{productId}/{categoryId}")]
        public async Task<ActionResult> Delete(long productId, long categoryId)
        {
            await Mediator.Send(new DeleteProductCategoryCommand
            {
                ProductId = productId,
                CategoryId = categoryId
            });
            return NoContent();
        }

    }
}
