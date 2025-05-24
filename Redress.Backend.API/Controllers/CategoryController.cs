using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.ListingArea.Categories;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.API.Controllers
{
    public class CategoryController : BaseController
    {
        [HttpGet("by-listing/{listingId}")]
        public async Task<ActionResult<CategoryTreeDto>> GetByListingId(Guid listingId)
        {
            var query = new GetCategoryByIdQuery { ListingId = listingId, UserId = UserId };
            return await Mediator.Send(query);
        }

        [HttpGet("by-sex/{sex}")]
        public async Task<ActionResult<List<CategoryTreeDto>>> GetBySex(Sex sex)
        {
            var query = new GetAllCategoriesBySexQuery { Sex = sex, UserId = UserId };
            var categories = await Mediator.Send(query);
            return Ok(categories);
        }

        [HttpGet("tree")]
        public async Task<ActionResult<IEnumerable<CategoryTreeDto>>> GetTree()
        {
            var query = new GetCategoryTreeQuery();
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CategoryCreateDto category)
        {
            var command = new CreateCategoryCommand
            {
                Category = category,
                UserId = UserId
            };
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteCategoryCommand { Id = id, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}