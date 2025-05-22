using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class BidDetailsMapping : AutoMapper.Profile
    {
        public BidDetailsMapping()
        {
            CreateMap<Bid, BidDetailsDto>()
                .IncludeBase<Bid, BidDto>()
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.Auction, opt => opt.MapFrom(src => src.Auction));
        }
    }
} 