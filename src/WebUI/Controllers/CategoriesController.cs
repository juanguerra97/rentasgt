using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Categories.Commands.CreateCategory;
using rentasgt.Application.Categories.Commands.DeleteCategory;
using rentasgt.Application.Categories.Commands.UpdateCategory;
using rentasgt.Application.Categories.Queries.GetCategories;
using rentasgt.Application.Categories.Queries.GetCategoryById;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{

    [Authorize(Policy = "OnlyAdmin")]
    public class CategoriesController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> Get(int pageSize = 15, int pageNumber = 1)
        {
            return await Mediator.Send(new GetCategoriesQuery(pageSize, pageNumber));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(long id)
        {
            return await Mediator.Send(new GetCategoryByIdQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateCategoryCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, UpdateCategoryCommand command)
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
            await Mediator.Send(new DeleteCategoryCommand { Id = id });
            return NoContent();
        }

    }
}
