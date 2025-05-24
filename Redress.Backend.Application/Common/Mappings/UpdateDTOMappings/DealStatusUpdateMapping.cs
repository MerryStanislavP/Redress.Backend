using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.UpdateDTOMappings
{
    public class DealStatusUpdateMapping : AutoMapper.Profile
    {
        public DealStatusUpdateMapping()
        {
            CreateMap<DealStatusUpdateDto, Deal>();
        }
    }
}
