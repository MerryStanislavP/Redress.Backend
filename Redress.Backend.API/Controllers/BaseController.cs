using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Redress.Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Redress.Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        private IUserContextService _userContextService;

        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IUserContextService UserContextService =>
            _userContextService ??= HttpContext.RequestServices.GetService<IUserContextService>();

        protected Guid UserId => UserContextService.GetCurrentUserId() ?? Guid.Empty;
    }
}
