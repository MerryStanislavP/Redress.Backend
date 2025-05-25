using Redress.Backend.Domain.Entities;
using AutoMapper;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;

namespace Redress.Backend.Application.Common.Mappings.ReadingDTOMappings
{
    public class ListingDetailsMapping : AutoMapper.Profile
    {
        public ListingDetailsMapping() {
            CreateMap<Listing, ListingDetailsDto>()
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.ListingImages));
            CreateMap<ListingImage, ListingImageDto>();
        }
    }
}
