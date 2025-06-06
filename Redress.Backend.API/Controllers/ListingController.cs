using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.ListingArea.Listings;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Application.Common.Models;
using Redress.Backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Redress.Backend.API.Controllers
{
    [Authorize]
    public class ListingController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ListingDetailsDto>> GetById(Guid id) 
        {
            var query = new GetListingByIdQuery { Id = id, UserId = UserId };
            var listing = await Mediator.Send(query);
            return Ok(listing);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] ListingCreateDto listing)
        {
            var command = new CreateListingCommand { Listing = listing, UserId = UserId };
            var listingId = await Mediator.Send(command);
            return Ok(listingId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ListingUpdateDto updateDto)
        {
            var command = new UpdateListingCommand 
            { 
                Id = id, 
                UserId = UserId,
                UpdateDto = updateDto
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteListingCommand { Id = id, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("by-profile")]
        public async Task<ActionResult<PaginatedList<ListingDto>>> GetByProfile(
            [FromQuery] Guid profileId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetListingsByProfileQuery
            {
                ProfileId = profileId,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var listings = await Mediator.Send(query);
            return Ok(listings);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ListingDto>>> GetByCategory(
            [FromQuery] Guid categoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetListingsByCategoryQuery
            {
                CategoryId = categoryId,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var listings = await Mediator.Send(query);
            return Ok(listings);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ListingDto>>> GetBySex(
            [FromQuery] Sex sex,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 28)
        {
            var query = new GetListingsBySexQuery
            {
                Sex = sex,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var listings = await Mediator.Send(query);
            return Ok(listings);
        }

        [HttpGet("by-price-range")]
        public async Task<ActionResult<PaginatedList<ListingDto>>> GetByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetListingsByPriceRangeQuery
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var listings = await Mediator.Send(query);
            return Ok(listings);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ListingDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllListingsQuery
            {
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var listings = await Mediator.Send(query);
            return Ok(listings);
        }
    }
} 