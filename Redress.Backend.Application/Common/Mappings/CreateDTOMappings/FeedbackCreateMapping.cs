using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.CreateDTOMappings
{
    public class FeedbackCreateMapping : AutoMapper.Profile
    {
        public FeedbackCreateMapping()
        {
            CreateMap<FeedbackCreateDto, Feedback>();
        }
    }
}
