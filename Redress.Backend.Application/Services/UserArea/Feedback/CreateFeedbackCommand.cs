using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class CreateFeedbackCommand : IRequest<Guid>, IOwnershipCheck
    {
        public FeedbackCreateDto Feedback { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var deal = await context.Deals
                .Include(d => d.Listing)
                .Include(d => d.Profile)
                .FirstOrDefaultAsync(d => d.Id == Feedback.DealId, cancellationToken);

            if (deal == null)
                return false;

            return deal.Profile.UserId == UserId;
        }
    }

    public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public CreateFeedbackCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            // Verify that deal exists and doesn't already have feedback
            var deal = await _context.Deals
                .Include(d => d.Feedback)
                .Include(d => d.Listing)
                    .ThenInclude(l => l.Profile)
                .FirstOrDefaultAsync(d => d.Id == request.Feedback.DealId, cancellationToken);

            if (deal == null)
                throw new KeyNotFoundException($"Deal with ID {request.Feedback.DealId} not found");

            if (deal.Feedback != null)
                throw new InvalidOperationException("This deal already has feedback");

            if (deal.Status != DealStatus.Completed)
                throw new InvalidOperationException("Cannot leave feedback for a deal that is not completed");

            var feedback = _mapper.Map<Domain.Entities.Feedback>(request.Feedback);

            feedback.CreatedAt = DateTime.UtcNow;

            await _context.Feedbacks.AddAsync(feedback, cancellationToken);

            // Update profile's average rating
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.Id == deal.Listing.Profile.Id, cancellationToken);

            if (profile != null)
            {
                var allFeedbacks = await _context.Deals
                    .Include(d => d.Feedback)
                    .Where(d => d.ProfileId == profile.Id && d.Feedback != null)
                    .Select(d => d.Feedback.Rating)
                    .ToListAsync(cancellationToken);

                profile.AverageRating = allFeedbacks.Any() ? allFeedbacks.Average() : 0;
                profile.RatingCount = allFeedbacks.Count;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return feedback.Id;
        }
    }
} 