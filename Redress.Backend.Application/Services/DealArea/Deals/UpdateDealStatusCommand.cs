using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class UpdateDealStatusCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DealStatusUpdateDto UpdateDto { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

            if (user == null)
                return false;

            // Admin can update any deal
            if (user.Role == UserRole.Admin)
                return true;

            // Get the deal with related entities
            var deal = await context.Deals
                .Include(d => d.Listing)
                    .ThenInclude(l => l.Profile)
                .FirstOrDefaultAsync(d => d.Id == Id, cancellationToken);

            if (deal == null)
                return false;

            // Only the listing owner can update deal status
            return deal.Listing?.Profile?.UserId == UserId;
        }
    }

    public class UpdateDealStatusCommandHandler : IRequestHandler<UpdateDealStatusCommand>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public UpdateDealStatusCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateDealStatusCommand request, CancellationToken cancellationToken)
        {
            var deal = await _context.Deals
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (deal == null)
                throw new KeyNotFoundException($"Deal with ID {request.Id} not found");

            _mapper.Map(request.UpdateDto, deal);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
