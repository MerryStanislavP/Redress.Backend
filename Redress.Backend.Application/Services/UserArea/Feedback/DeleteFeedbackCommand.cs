using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class DeleteFeedbackCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteFeedbackCommandHandler : IRequestHandler<DeleteFeedbackCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteFeedbackCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Deal)
                .ThenInclude(d => d.Profile)
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (feedback == null)
                throw new KeyNotFoundException($"Feedback with ID {request.Id} not found");

            // Store profile reference before removing feedback
            var profile = feedback.Deal?.Profile;

            // Remove the feedback
            _context.Feedbacks.Remove(feedback);

            // Update profile's average rating if profile exists
            if (profile != null)
            {
                var allFeedbacks = await _context.Deals
                    .Include(d => d.Feedback)
                    .Where(d => d.ProfileId == profile.Id && d.Feedback != null)
                    .Select(d => d.Feedback.Rating)
                    .ToListAsync(cancellationToken);

                profile.AverageRating = allFeedbacks.Any() ? allFeedbacks.Average() : null;
                profile.RatingCount = allFeedbacks.Count;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 