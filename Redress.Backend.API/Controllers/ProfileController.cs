using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.UserArea.Profiles;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.API.Controllers
{
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

        [HttpDelete("{id}/image")]
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

        [HttpGet("user")]
        public async Task<ActionResult<ProfileDetailsDto>> GetUserProfile()
        {
            var query = new GetUserProfileQuery { UserId = UserId };
            var profile = await Mediator.Send(query);
            return Ok(profile);
        }
    }
} 