using Application.RatingToProducts.Commands.IgnoreRating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rentasgt.Application.Common.Models;
using rentasgt.Application.RatingToProducts.Commands.RateProduct;
using rentasgt.Application.RatingToProducts.Queries.GetPendingRatingToProduct;
using rentasgt.Application.RatingToProducts.Queries.GetRatingsToProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Controllers
{
    [Authorize]
    public class RatingToProductsController : ApiController
    {

        [HttpGet("pending")]
        public async Task<ActionResult<RatingToProductDto>> GetPending()
        {
            return await Mediator.Send(new GetPendingRatingToProductQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaginatedListResponse<RatingToProductDto>>> GetRatingsForProduct(long id, int pageNumber = 1, int pageSize = 15)
        {
            return await Mediator.Send(new GetRatingsToProductQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                ProductId = id
            });
        }

        [HttpPut("{id}/rate")]
        public async Task<ActionResult<double>> Rate(long id, RateProductCommand command)
        {
            if (id != command.RatingId)
            {
                return BadRequest(new { message = "Los ID no coinciden" });
            }
            return await Mediator.Send(command);
        }

        [HttpPut("{id}/ignore")]
        public async Task<ActionResult> Ignore(long id)
        {
            await Mediator.Send(new IgnoreRatingCommand { RatingId = id });
            return NoContent();
        }

    }
}
