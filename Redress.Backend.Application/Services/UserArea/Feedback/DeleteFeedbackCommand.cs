using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class DeleteFeedbackCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

            if (user == null)
                return false;

            // Admin can delete any feedback
            if (user.Role == UserRole.Admin)
                return true;

            // Get the feedback with related entities
            var feedback = await context.Feedbacks
                .Include(f => f.Deal)
                .ThenInclude(d => d.Profile)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(f => f.Id == Id, cancellationToken);

            if (feedback == null)
                return false;

            // Only the user who owns the profile that received the feedback can delete it
            return feedback.Deal?.Profile?.UserId == UserId;
        }
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