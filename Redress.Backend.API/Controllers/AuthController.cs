using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.Auth.Register;
using Redress.Backend.Contracts.DTOs.CreateDTOs;

namespace Redress.Backend.API.Controllers
{
    public class AuthController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Register([FromBody] UserCreateDto user)
        {
            var command = new RegisterUserCommand { User = user };
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }
    }
} 