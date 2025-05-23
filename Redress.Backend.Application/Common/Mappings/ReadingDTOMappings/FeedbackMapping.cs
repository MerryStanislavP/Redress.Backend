﻿using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class FeedbackMapping : AutoMapper.Profile
    {
        public FeedbackMapping()
        {
            CreateMap<Feedback, FeedbackDto>();
        }
    }
}
