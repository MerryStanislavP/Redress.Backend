using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class GetFeedbackByIdQuery : IRequest<FeedbackDetailsDto>
    {
        public Guid Id { get; set; }
    }

    public class GetFeedbackByIdQueryHandler : IRequestHandler<GetFeedbackByIdQuery, FeedbackDetailsDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetFeedbackByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FeedbackDetailsDto> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Deal)
                .ThenInclude(d => d.Profile)
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (feedback == null)
                throw new KeyNotFoundException($"Feedback with ID {request.Id} not found");

            return _mapper.Map<FeedbackDetailsDto>(feedback);
        }
    }
} 