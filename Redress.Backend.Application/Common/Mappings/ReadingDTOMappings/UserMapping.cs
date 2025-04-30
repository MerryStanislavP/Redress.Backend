using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class UserMapping : AutoMapper.Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>();
        }
    }
}
