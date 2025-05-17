using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class FeedbackDetailsMapping : AutoMapper.Profile
    {
        public FeedbackDetailsMapping()
        {
            CreateMap<Feedback, FeedbackDetailsDto>()
                .ForMember(dest => dest.Deal, opt => opt.MapFrom(src => src.Deal))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Deal.Profile));
        }
    }
} 