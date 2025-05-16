using Redress.Backend.Contracts.DTOs.ReadingDTO;
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
