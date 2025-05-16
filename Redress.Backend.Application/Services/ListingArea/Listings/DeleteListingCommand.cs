using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Behavior;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class DeleteListingCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var listing = await context.Listings
                .Include(l => l.Profile)
                    .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(l => l.Id == Id, cancellationToken);

            if (listing == null)
                return false;

            // Проверяем, является ли пользователь владельцем листинга
            if (listing.Profile?.User?.Role == UserRole.Admin)
            {
                return true;
            }
            return listing.Profile?.UserId == UserId;
        }
    }

    public class DeleteListingCommandHandler : IRequestHandler<DeleteListingCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteListingCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await _context.Listings
                .Include(l => l.Profile)
                .Include(l => l.ListingImages)
                .Include(l => l.Favorites)
                .Include(l => l.Auction)
                .Include(l => l.Deal)
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Листинг не найден: {request.Id}");

            // Проверка на активные сделки и аукционы уже выполнена в CheckOwnershipAsync
            // Здесь только бизнес-логика удаления

            // Check if listing is part of an active deal
            if (listing.Deal != null && listing.Deal.Status != Domain.Enums.DealStatus.Rejected)
                throw new InvalidOperationException("Нельзя удалить листинг, который является частью активной сделки");

            // Check if listing is part of an active auction
            if (listing.Auction != null && listing.Auction.EndAt > DateTime.UtcNow)
                throw new InvalidOperationException("Нельзя удалить листинг, который является частью активного аукциона");

            // Remove related entities
            _context.ListingImages.RemoveRange(listing.ListingImages);
            _context.Favorites.RemoveRange(listing.Favorites);
            
            if (listing.Auction != null)
                _context.Auctions.Remove(listing.Auction);
            
            if (listing.Deal != null)
                _context.Deals.Remove(listing.Deal);

            // Remove the listing itself
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 