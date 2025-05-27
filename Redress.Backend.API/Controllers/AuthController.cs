using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.Auth.Register;
using Redress.Backend.Application.Services.Auth.Login;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.AuthDTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Redress.Backend.API.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Register([FromBody] UserCreateDto user)
        {
            var command = new RegisterUserCommand { User = user };
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }

        [HttpPost]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var command = new LoginCommand { LoginDto = loginDto };
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var command = new RefreshTokenCommand { RefreshTokenDto = refreshTokenDto };
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
} 