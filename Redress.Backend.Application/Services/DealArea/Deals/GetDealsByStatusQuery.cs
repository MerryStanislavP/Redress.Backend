using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class GetDealsByStatusQuery : IRequest<PaginatedList<DealDto>>
    {
        public DealStatus Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }
    }

    public class GetDealsByStatusQueryHandler : IRequestHandler<GetDealsByStatusQuery, PaginatedList<DealDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetDealsByStatusQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DealDto>> Handle(GetDealsByStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // Базовый запрос с фильтрацией по статусу
            var query = _context.Deals
                .Include(d => d.Listing)
                .Where(d => d.Status == request.Status);

            // Если пользователь не админ/модератор, показываем только его сделки
            if (user.Role != UserRole.Admin && user.Role != UserRole.Moderator)
            {
                query = query.Where(d => d.Profile.UserId == request.UserId);
            }

            query = query.OrderByDescending(d => d.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<DealDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<DealDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
} 