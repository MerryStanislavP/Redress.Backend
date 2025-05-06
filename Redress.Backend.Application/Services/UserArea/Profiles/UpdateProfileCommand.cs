using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Profiles
{
    public class UpdateProfileCommand : IRequest
    {
        public Guid Id { get; set; }
        public ProfileUpdateDto UpdateDto { get; set; }
    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProfileCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile with ID {request.Id} not found");

            _mapper.Map(request.UpdateDto, profile);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
