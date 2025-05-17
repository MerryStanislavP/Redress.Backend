using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class GetDealByIdQuery : IRequest<DealDetailsDto>, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

            if (user == null)
                return false;

            var deal = await context.Deals
                .Include(d => d.Listing)
                .ThenInclude(l => l.Profile)
                .Include(d => d.Profile)
                .FirstOrDefaultAsync(d => d.Id == Id, cancellationToken);

            if (deal == null)
                return false;

            // Admin can access any deal
            if (user.Role == Domain.Enums.UserRole.Admin)
                return true;

            // User can access if they are either the buyer or seller
            return deal.Profile.UserId == UserId || deal.Listing.Profile.UserId == UserId;
        }
    }

    public class GetDealByIdQueryHandler : IRequestHandler<GetDealByIdQuery, DealDetailsDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetDealByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DealDetailsDto> Handle(GetDealByIdQuery request, CancellationToken cancellationToken)
        {
            var deal = await _context.Deals
                .Include(d => d.Listing)
                .Include(d => d.Profile)
                .Include(d => d.Feedback)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (deal == null)
                throw new KeyNotFoundException($"Deal with ID {request.Id} not found");

            return _mapper.Map<DealDetailsDto>(deal);
        }
    }
} 