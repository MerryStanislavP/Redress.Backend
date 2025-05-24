using MediatR;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace Redress.Backend.Application.Services.UserArea.Users
{
    public class UpdateUserCommand : IRequest, IRequireRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserUpdateDto UpdateDto { get; set; }

        public UserRole RequiredRole => UserRole.Admin;
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            // Проверяем уникальность email, если он обновляется
            if (!string.IsNullOrEmpty(request.UpdateDto.Email) && request.UpdateDto.Email != user.Email)
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == request.UpdateDto.Email, cancellationToken);

                if (emailExists)
                    throw new InvalidOperationException($"User with email {request.UpdateDto.Email} already exists");
            }

            // Обновляем только те поля, которые были предоставлены
            if (!string.IsNullOrEmpty(request.UpdateDto.Username))
                user.Username = request.UpdateDto.Username;

            if (!string.IsNullOrEmpty(request.UpdateDto.Email))
                user.Email = request.UpdateDto.Email;

            if (!string.IsNullOrEmpty(request.UpdateDto.PhoneNumber))
                user.PhoneNumber = request.UpdateDto.PhoneNumber;

            // Если предоставлен новый пароль, хешируем его
            if (!string.IsNullOrEmpty(request.UpdateDto.PasswordHash))
            {
                user.PasswordHash = HashPassword(request.UpdateDto.PasswordHash);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 