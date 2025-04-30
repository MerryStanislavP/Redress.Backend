using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class AuctionMapping : AutoMapper.Profile
    {
        public AuctionMapping()
        {
            CreateMap<Auction, AuctionDto>();
        }
    }
}
