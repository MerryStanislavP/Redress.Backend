using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class DealDetailsMapping : AutoMapper.Profile
    {
        public DealDetailsMapping()
        {
            CreateMap<Deal, DealDetailsDto>()
                .ForMember(dest => dest.Listing, opt => opt.MapFrom(src => src.Listing))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.Feedback));
        }
    }
} 