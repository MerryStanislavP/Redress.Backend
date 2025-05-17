using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.CreateDTOMappings
{
    public class FavoriteCreateMapping : AutoMapper.Profile
    {
        public FavoriteCreateMapping()
        {
            CreateMap<FavoriteCreateDto, Favorite>();
        }
    }
}
