using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class ListingImageMapping : AutoMapper.Profile
    {
        public ListingImageMapping()
        {
            CreateMap<ListingImage, ListingImageDto>();
        }
    }
}
