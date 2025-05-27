using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.AuctionArea.Auctions;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;

namespace Redress.Backend.API.Controllers
{
    [Authorize]
    public class AuctionController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDetailsDto>> GetById(Guid id)
        {
            var query = new GetAuctionByIdQuery { Id = id, UserId = UserId };
            var auction = await Mediator.Send(query);
            return Ok(auction);
        }

        [HttpPost("{listingId}")]
        public async Task<ActionResult<Guid>> StartAuction(Guid listingId, [FromBody] AuctionCreateDto auction)
        {
            var command = new StartAuctionCommand
            {
                ListingId = listingId,
                Auction = auction,
                UserId = UserId
            };
            var auctionId = await Mediator.Send(command);
            return Ok(auctionId);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteAuctionCommand { Id = id, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<ActionResult<PaginatedList<AuctionDto>>> GetActiveAuctions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetActiveAuctionsQuery
            {
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var auctions = await Mediator.Send(query);
            return Ok(auctions);
        }
    }
} 