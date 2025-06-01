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
                    opt => opt.MapFrom(src => src.ListingImages))
                .ForMember(dest => dest.AuctionId,
                    opt => opt.MapFrom(src => src.IsAuction ? src.Auction.Id : (Guid?)null));
            CreateMap<ListingImage, ListingImageDto>();
        }
    }
}
