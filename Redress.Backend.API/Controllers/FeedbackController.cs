using Microsoft.AspNetCore.Mvc;
using Redress.Backend.Application.Services.UserArea.Feedback;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;

namespace Redress.Backend.API.Controllers
{
    [Authorize]
    public class FeedbackController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDetailsDto>> GetById(Guid id)
        {
            var query = new GetFeedbackByIdQuery { Id = id};
            var feedback = await Mediator.Send(query);
            return Ok(feedback);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] FeedbackCreateDto feedback)
        {
            var command = new CreateFeedbackCommand
            {
                Feedback = feedback,
                UserId = UserId
            };
            var feedbackId = await Mediator.Send(command);
            return Ok(feedbackId);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteFeedbackCommand { Id = id, UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("profile")]
        public async Task<ActionResult<PaginatedList<FeedbackDto>>> GetProfileFeedbacks(
            [FromQuery] Guid profileId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetProfileFeedbacksQuery
            {
                ProfileId = profileId,
                Page = page,
                PageSize = pageSize,
                UserId = UserId
            };
            var feedbacks = await Mediator.Send(query);
            return Ok(feedbacks);
        }
    }
} 