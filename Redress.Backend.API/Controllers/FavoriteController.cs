using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.ListingArea.Favorites;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.API.Controllers
{
    public class FavoriteController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> AddToFavorites([FromBody] FavoriteCreateDto favorite)
        {
            var command = new AddToFavoritesCommand 
            { 
                Favorite = favorite,
                UserId = UserId
            };
            var favoriteId = await Mediator.Send(command);
            return Ok(favoriteId);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFromFavorites(
            [FromQuery] Guid profileId,
            [FromQuery] Guid listingId)
        {
            var command = new DeleteFavoriteCommand 
            { 
                ProfileId = profileId,
                ListingId = listingId,
                UserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ListingDto>>> GetUserFavorites(
            [FromQuery] Guid profileId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUserFavoritesQuery
            {
                ProfileId = profileId,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var favorites = await Mediator.Send(query);
            return Ok(favorites);
        }
    }
} 