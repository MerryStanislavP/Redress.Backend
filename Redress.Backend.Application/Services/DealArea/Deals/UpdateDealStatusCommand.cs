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

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class UpdateDealStatusCommand : IRequest
    {
        public Guid DealId { get; set; }
        public DealUpdateDto UpdateDto { get; set; }
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
                .FirstOrDefaultAsync(d => d.Id == request.DealId, cancellationToken);

            if (deal == null)
                throw new KeyNotFoundException($"Deal with ID {request.DealId} not found");

            _mapper.Map(request.UpdateDto, deal);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
