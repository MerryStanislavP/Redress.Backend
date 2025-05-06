using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.UpdateDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class UpdateListingCommand : IRequest
    {
        public Guid Id { get; set; }
        public ListingUpdateDto UpdateDto { get; set; }
    }

    public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public UpdateListingCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await _context.Listings
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.Id} not found");

            // If category is being updated, verify it exists
            if (request.UpdateDto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == request.UpdateDto.CategoryId.Value, cancellationToken);

                if (!categoryExists)
                    throw new KeyNotFoundException($"Category with ID {request.UpdateDto.CategoryId} not found");
            }

            _mapper.Map(request.UpdateDto, listing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
