using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class LeaveFeedbackCommand : IRequest<Guid>, IOwnershipCheck
    {
        public FeedbackCreateDto Feedback { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            // Check if the deal exists
            var deal = await context.Deals
                .Include(d => d.Listing)
                .ThenInclude(l => l.Profile)
                .FirstOrDefaultAsync(d => d.Id == Feedback.DealId, cancellationToken);

            if (deal == null)
                return false;

            // The user must be the owner of the listing to leave feedback
            // (feedback is left for the buyer by the seller)
            return deal.Listing?.Profile?.UserId == UserId;
        }
    }

    public class LeaveFeedbackCommandHandler : IRequestHandler<LeaveFeedbackCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public LeaveFeedbackCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(LeaveFeedbackCommand request, CancellationToken cancellationToken)
        {
            // Verify that deal exists and doesn't already have feedback
            var deal = await _context.Deals
                .Include(d => d.Feedback)
                .FirstOrDefaultAsync(d => d.Id == request.Feedback.DealId, cancellationToken);

            if (deal == null)
                throw new KeyNotFoundException($"Deal with ID {request.Feedback.DealId} not found");

            if (deal.Feedback != null)
                throw new InvalidOperationException("This deal already has feedback");

            var feedback = _mapper.Map<Domain.Entities.Feedback>(request.Feedback);
            feedback.CreatedAt = DateTime.UtcNow;

            await _context.Feedbacks.AddAsync(feedback, cancellationToken);

            // Update profile's average rating
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.Id == deal.ProfileId, cancellationToken);

            if (profile != null)
            {
                var allFeedbacks = await _context.Deals
                    .Include(d => d.Feedback)
                    .Where(d => d.ProfileId == profile.Id && d.Feedback != null)
                    .Select(d => d.Feedback.Rating)
                    .ToListAsync(cancellationToken);

                profile.AverageRating = allFeedbacks.Average();
                profile.RatingCount = allFeedbacks.Count;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return feedback.Id;
        }
    }
}
