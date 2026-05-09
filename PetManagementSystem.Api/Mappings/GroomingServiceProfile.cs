using AutoMapper;
using PetManagementSystem.Api.DTOs.GroomingServiceDtos;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class GroomingServiceProfile : Profile
    {
        public GroomingServiceProfile()
        {
            CreateMap<GroomingService, GroomingServiceDto>().ReverseMap();
            CreateMap<CreateGroomingServiceDto, GroomingService>();
            CreateMap<UpdateGroomingServiceDto, GroomingService>().ReverseMap();
        }
    }
}
