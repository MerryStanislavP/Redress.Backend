using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.ListingArea.Images;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Application.Common.Models;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;

namespace Redress.Backend.API.Controllers
{
    public class ListingImageController : BaseController
    {
        [HttpGet("{listingId}")]
        public async Task<ActionResult<PaginatedList<ListingImageDto>>> GetImages(
            Guid listingId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 3)
        {
            var query = new GetListingImagesQuery
            {
                ListingId = listingId,
                UserId = UserId,
                Page = page,
                PageSize = pageSize
            };
            var images = await Mediator.Send(query);
            return Ok(images);
        }

        //[HttpPost("{listingId}")]
        //public async Task<ActionResult<Guid>> UploadImage(Guid listingId, IFormFile file)
        //{
        //    // Create a temporary file path
        //    var tempPath = Path.GetTempFileName();
        //    using (var stream = new FileStream(tempPath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    // TODO: Upload file to cloud storage and get URL
        //    var imageUrl = "https://example.com/images/" + file.FileName; // Replace with actual cloud storage URL

        //    var command = new UploadListingImageCommand
        //    {
        //        Image = new ListingImageCreateDto
        //        {
        //            Name = file.FileName,
        //            Url = imageUrl,
        //            ListingId = listingId
        //        },
        //        UserId = UserId
        //    };
        //    var imageId = await Mediator.Send(command);
        //    return Ok(imageId);
        //}

        [HttpDelete("{listingId}/{imageId}")]
        public async Task<ActionResult> DeleteImage(Guid listingId, Guid imageId)
        {
            var command = new DeleteListingImageCommand
            {
                Id = imageId,
                UserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
} 