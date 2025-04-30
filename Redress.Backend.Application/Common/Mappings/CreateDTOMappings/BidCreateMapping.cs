using Redress.Backend.Contracts.DTOs.CreateDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.CreateDTOMappings
{
    public class BidCreateMapping : AutoMapper.Profile
    {
        public BidCreateMapping()
        {
            CreateMap<BidCreateDto, Bid>();
        }
    }
}
