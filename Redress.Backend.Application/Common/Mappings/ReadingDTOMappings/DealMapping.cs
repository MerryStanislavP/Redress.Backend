using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class DealMapping : AutoMapper.Profile
    {
        public DealMapping()
        {
            CreateMap<Deal, DealDto>();
        }
    }
}
