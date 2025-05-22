using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.UserArea.Users;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Enums;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.API.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var query = new GetUserByIdQuery { Id = id, UserId = UserId };
            var user = await Mediator.Send(query);
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<UserDto>>> GetByRole(
            [FromQuery] UserRole role,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersByRoleQuery 
            { 
                Role = role,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var users = await Mediator.Send(query);
            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand { Id = id, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }
    }
} 