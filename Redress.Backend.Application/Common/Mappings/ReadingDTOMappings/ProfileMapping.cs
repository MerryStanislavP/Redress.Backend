using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class ProfileMapping : AutoMapper.Profile
    {
        public ProfileMapping()
        {
            CreateMap<Domain.Entities.Profile, ProfileDto>();
        }
    }
}
