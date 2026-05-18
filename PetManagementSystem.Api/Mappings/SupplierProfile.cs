using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class GroomingServiceProfile : Profile
    {
        public GroomingServiceProfile()
        {
            CreateMap<GroomingService, GroomingDTO>().ReverseMap();
        }
    }
}