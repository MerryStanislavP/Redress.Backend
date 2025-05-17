using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class CategoryMapping : AutoMapper.Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
