using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redress.Backend.Domain.Enums;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Application.Common;
using Redress.Backend.Application.Common.Behavior;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class CreateListingCommand : IRequest<Guid>, IRequireRole
    {
        public ListingCreateDto Listing { get; set; }
        public Guid UserId { get; set; }

        public UserRole RequiredRole => UserRole.Regular;
    }

    public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public CreateListingCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
        {
            var listing = _mapper.Map<Listing>(request.Listing);
            
            // Проверка на существование профиля уже выполнена в CheckOwnershipAsync
            // Здесь только проверка категории и создание листинга
            
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.Listing.CategoryId, cancellationToken);
            
            if (!categoryExists)
                throw new KeyNotFoundException($"Категория не найдена: {request.Listing.CategoryId}");

            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"Пользователь не найден: {request.UserId}");

            // Установка статуса и времени создания
            listing.Status = ListingStatus.Active;
            listing.CreatedAt = DateTime.UtcNow;
            listing.IsAuction = false;
            listing.ProfileId = user.Profile.Id;

            await _context.Listings.AddAsync(listing, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return listing.Id;
        }
    }
}
