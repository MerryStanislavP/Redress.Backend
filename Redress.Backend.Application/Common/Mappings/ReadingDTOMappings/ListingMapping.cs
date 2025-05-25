using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class ListingMapping : AutoMapper.Profile
    {
        public ListingMapping()
        {
            CreateMap<Listing, ListingDto>()
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src =>
                        src.ListingImages
                            .OrderBy(img => img.CreatedAt)
                            .Select(img => img.Url)
                            .FirstOrDefault()));
        }
    }
}
