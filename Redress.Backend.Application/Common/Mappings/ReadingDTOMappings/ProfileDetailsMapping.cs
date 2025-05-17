using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class ProfileDetailsMapping : AutoMapper.Profile
    {
        public ProfileDetailsMapping()
        {
            CreateMap<Domain.Entities.Profile, ProfileDetailsDto>();
        }
    }
} 