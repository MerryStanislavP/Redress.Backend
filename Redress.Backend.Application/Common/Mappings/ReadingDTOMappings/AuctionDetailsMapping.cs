using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class AuctionDetailsMapping : AutoMapper.Profile
    {
        public AuctionDetailsMapping()
        {
            CreateMap<Auction, AuctionDetailsDto>()
                .IncludeBase<Auction, AuctionDto>();
        }
    }
} 