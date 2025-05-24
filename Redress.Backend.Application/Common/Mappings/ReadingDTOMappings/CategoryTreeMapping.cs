using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class CategoryTreeMapping : AutoMapper.Profile
    {
        public CategoryTreeMapping()
        {
            CreateMap<Category, CategoryTreeDto>();
        }
    }
} 