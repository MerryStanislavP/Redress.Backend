using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.Auth.Register;
using Redress.Backend.Application.Services.Auth.Login;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.AuthDTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IGoogleAuthService _googleAuthService;

        public AuthController(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

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

        [HttpPost]
        public async Task<ActionResult<AuthResponseDto>> GoogleAuth([FromBody] GoogleAuthDto request, CancellationToken cancellationToken)
        {
            try
            {
                var (accessToken, refreshToken) = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken, cancellationToken);

                return Ok(new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60) // Token expiration time
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 