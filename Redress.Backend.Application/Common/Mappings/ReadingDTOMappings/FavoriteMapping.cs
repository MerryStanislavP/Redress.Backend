﻿using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class FavoriteMapping : AutoMapper.Profile
    {
        public FavoriteMapping()
        {
            CreateMap<Favorite, FavoriteDto>();
        }
    }
}
