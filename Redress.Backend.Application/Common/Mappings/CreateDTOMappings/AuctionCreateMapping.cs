using Redress.Backend.Contracts.DTOs.CreateDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.CreateDTOMappings
{
    public class AuctionCreateMapping : AutoMapper.Profile
    {
        public AuctionCreateMapping()
        {
            CreateMap<AuctionCreateDto, Auction>();
        }
    }
}
