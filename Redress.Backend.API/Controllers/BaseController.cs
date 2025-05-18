using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Redress.Backend.API.Controllers
{
        [ApiController]
        [Route("api/[controller]/[action]")]
        public abstract class BaseController : ControllerBase
        {
            private IMediator _mediator;
            protected IMediator Mediator =>
                _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        //internal Guid UserId => !User.Identity.IsAuthenticated
        //    ? Guid.Empty
        //    : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        internal Guid UserId => Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d");

    }

}
