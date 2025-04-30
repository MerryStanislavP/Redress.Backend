using Redress.Backend.Contracts.DTOs.CreateDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.CreateDTOMappings
{
    public class DealCreateMapping : AutoMapper.Profile
    {
        public DealCreateMapping()
        {
            CreateMap<DealCreateDto, Deal>();
        }
    }

}
