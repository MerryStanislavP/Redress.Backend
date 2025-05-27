using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.ListingArea.Images;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Application.Common.Models;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Redress.Backend.API.Controllers
{
    [Authorize]
    public class ListingImageController : BaseController
    {
        [HttpGet("{listingId}")]
        public async Task<ActionResult<List<ListingImageDto>>> GetImages(Guid listingId)
        {
            var query = new GetListingImagesQuery { ListingId = listingId, UserId = UserId };
            var images = await Mediator.Send(query);
            return Ok(images);
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Guid>> UploadImage([FromForm] IFormFile image, [FromForm]  Guid listingId)
        {
            var command = new UploadListingImageCommand
            {
                Image = image,
                ListingId = listingId,
                UserId = UserId
            };

            var imageId = await Mediator.Send(command);
            return Ok(imageId);
        }

        [HttpDelete("{imageId}")]
        public async Task<ActionResult> DeleteImage(Guid imageId)
        {
            var command = new DeleteListingImageCommand { Id = imageId, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }
    }
} 