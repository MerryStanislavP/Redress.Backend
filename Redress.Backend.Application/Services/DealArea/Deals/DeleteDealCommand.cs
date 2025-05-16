using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class DeleteDealCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var deal = await context.Deals
                .Include(d => d.Listing)
                .ThenInclude(l => l.Profile)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(d => d.Id == Id, cancellationToken);

            if (deal == null)
                return false;

            // Admin can delete any deal
            if (deal.Listing?.Profile?.User?.Role == Domain.Enums.UserRole.Admin)
                return true;

            // Owner of the listing can delete deals related to their listings
            return deal.Listing?.Profile?.UserId == UserId;
        }
    }

    public class DeleteDealCommandHandler : IRequestHandler<DeleteDealCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteDealCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteDealCommand request, CancellationToken cancellationToken)
        {
            var deal = await _context.Deals
                .Include(d => d.Feedback)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (deal == null)
                throw new KeyNotFoundException($"Deal with ID {request.Id} not found");

            // Check if deal is active
            if (deal.Status == Domain.Enums.DealStatus.Pending)
                throw new InvalidOperationException("Cannot delete an active deal");

            // Remove related feedback if exists
            if (deal.Feedback != null)
            {
                _context.Feedbacks.Remove(deal.Feedback);
            }

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 