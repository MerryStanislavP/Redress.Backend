using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class DeleteAuctionCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // ID пользователя, выполняющего удаление
    }

    public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteAuctionCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
        {
            // Получаем пользователя для проверки прав
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // Проверяем, что пользователь - администратор или модератор
            if (user.Role != UserRole.Admin && user.Role != UserRole.Moderator)
                throw new UnauthorizedAccessException("Only administrators and moderators can delete auctions");

            var auction = await _context.Auctions
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (auction == null)
                throw new KeyNotFoundException($"Auction with ID {request.Id} not found");

            // Check if auction is active
            if (auction.EndAt > DateTime.UtcNow)
                throw new InvalidOperationException("Cannot delete an active auction");

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 