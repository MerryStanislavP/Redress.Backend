using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.UserArea.Profiles;
using Redress.Backend.Application.Services.UserArea.Images;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;

namespace Redress.Backend.API.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDetailsDto>> GetById(Guid id) 
        {
            var query = new GetProfileByIdQuery { Id = id};
            var profile = await Mediator.Send(query);
            return Ok(profile);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ProfileUpdateDto updateDto)
        {
            var command = new UpdateProfileCommand
            {
                Id = id,
                UpdateDto = updateDto,
                UserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImage(Guid id)
        {
            var command = new DeleteProfileImageCommand
            {
                ProfileId = id,
                UserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Guid>> UploadImage([FromForm] IFormFile image, [FromForm]  Guid profileId)
        {
            var command = new UploadProfileImageCommand
            {
                Image = image,
                ProfileId = profileId,
                UserId = UserId
            };

            var imageId = await Mediator.Send(command);
            return Ok(imageId);
        }

        [HttpGet] 
        public async Task<ActionResult<ProfileDetailsDto>> GetUserProfile()
        {
            var query = new GetUserProfileQuery { UserId = UserId };
            var profile = await Mediator.Send(query);
            return Ok(profile);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetProfileImageUrl(Guid id)
        {
            var url = await Mediator.Send(new GetProfileImageCommand { ProfileId = id, UserId = UserId});
            return Ok(url);
        }
    }
} 