using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
