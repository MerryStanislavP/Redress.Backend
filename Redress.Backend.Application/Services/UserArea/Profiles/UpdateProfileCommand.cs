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
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.UserArea.Profiles
{
    public class UpdateProfileCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ProfileUpdateDto UpdateDto { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

            if (user == null)
                return false;

            // Admin can update any profile
            if (user.Role == UserRole.Admin)
                return true;

            // Get the profile
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == Id, cancellationToken);

            if (profile == null)
                return false;

            // User can only update their own profile
            return profile.UserId == UserId;
        }
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

            // Update only provided fields
            if (request.UpdateDto.Balance.HasValue)
                profile.Balance = request.UpdateDto.Balance.Value;

            if (!string.IsNullOrEmpty(request.UpdateDto.Bio))
                profile.Bio = request.UpdateDto.Bio;

            if (request.UpdateDto.Latitude.HasValue)
                profile.Latitude = request.UpdateDto.Latitude.Value;

            if (request.UpdateDto.Longitude.HasValue)
                profile.Longitude = request.UpdateDto.Longitude.Value;

            if (request.UpdateDto.RatingCount.HasValue)
                profile.RatingCount = request.UpdateDto.RatingCount.Value;

            if (request.UpdateDto.RatingStatus.HasValue)
                profile.RatingStatus = request.UpdateDto.RatingStatus.Value;

            if (request.UpdateDto.AverageRating.HasValue)
                profile.AverageRating = request.UpdateDto.AverageRating.Value;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
