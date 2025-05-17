using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.UpdateDTOMappings
{
    public class DealUpdateMapping : AutoMapper.Profile
    {
        public DealUpdateMapping()
        {
            CreateMap<DealUpdateDto, Deal>();
        }
    }
}
