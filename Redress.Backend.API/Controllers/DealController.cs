using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.DealArea.Deals;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.API.Controllers
{
    public class DealController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<DealDetailsDto>> GetById(Guid id)
        {
            var query = new GetDealByIdQuery { Id = id, UserId = UserId };
            var deal = await Mediator.Send(query);
            return Ok(deal);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] DealCreateDto deal)
        {
            var command = new CreateDealCommand
            {
                Deal = deal,
                UserId = UserId
            };
            var dealId = await Mediator.Send(command);
            return Ok(dealId);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteDealCommand { Id = id, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("user")]
        public async Task<ActionResult<PaginatedList<DealDto>>> GetUserDeals(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUserDealsQuery
            {
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var deals = await Mediator.Send(query);
            return Ok(deals);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(Guid id, [FromBody] DealUpdateDto updateDto)
        {
            var command = new UpdateDealStatusCommand
            {
                DealId = id,
                UserId = UserId,
                UpdateDto = updateDto
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("profile/{profileId}")]
        public async Task<ActionResult<PaginatedList<DealDto>>> GetDealsByProfile(
            Guid profileId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetDealsByProfileIdQuery
            {
                ProfileId = profileId,
                Page = page,
                PageSize = pageSize
            };
            var deals = await Mediator.Send(query);
            return Ok(deals);
        }
    }
} 