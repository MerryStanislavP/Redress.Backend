using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.AuctionArea.Bids;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.API.Controllers
{
    public class BidController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] BidCreateDto bid)
        {
            var command = new CreateBidCommand
            {
                Bid = bid,
                UserId = UserId
            };
            var bidId = await Mediator.Send(command);
            return Ok(bidId);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteBidCommand { Id = id, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("auction/{auctionId}")]
        public async Task<ActionResult<PaginatedList<BidDto>>> GetByAuction(
            Guid auctionId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetBidsByAuctionQuery
            {
                AuctionId = auctionId,
                Page = page,
                PageSize = pageSize
            };
            var bids = await Mediator.Send(query);
            return Ok(bids);
        }

        [HttpGet("profile/{profileId}")]
        public async Task<ActionResult<PaginatedList<BidDto>>> GetByProfile(
            Guid profileId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetBidsByProfileQuery
            {
                ProfileId = profileId,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var bids = await Mediator.Send(query);
            return Ok(bids);
        }
    }
} 