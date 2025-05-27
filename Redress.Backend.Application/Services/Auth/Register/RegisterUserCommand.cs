using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.Auth.Register
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public UserCreateDto User { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user with same email already exists
            var userExists = await _context.Users
                .AnyAsync(u => u.Email == request.User.Email, cancellationToken);

            if (userExists)
                throw new InvalidOperationException($"User with email {request.User.Email} already exists");

            // Create user
            var user = _mapper.Map<User>(request.User);
            
            // Hash password using BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.User.PasswordHash);

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var profile = new Domain.Entities.Profile
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Profiles.AddAsync(profile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
