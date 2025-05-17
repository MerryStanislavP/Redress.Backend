using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class ProfileImageMapping : AutoMapper.Profile
    {
        public ProfileImageMapping()
        {
            CreateMap<ProfileImage, ProfileImageDto>();
        }
    }
}
