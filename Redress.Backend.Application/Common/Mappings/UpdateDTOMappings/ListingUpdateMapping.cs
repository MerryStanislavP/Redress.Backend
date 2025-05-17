using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings.UpdateDTOMappings
{
    public class ListingUpdateMapping : AutoMapper.Profile
    {
        public ListingUpdateMapping()
        {
            CreateMap<ListingUpdateDto, Listing>();
        }
    }
}
