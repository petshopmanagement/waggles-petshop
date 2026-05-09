using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;
namespace PetManagementSystem.Api.Mappings
{
    public class VaccinationProfile : Profile
    {
        public VaccinationProfile()
        {
            CreateMap<Vaccination, VaccinationDto>().ReverseMap();
            CreateMap<CreateVaccinationDto, Vaccination>();
            CreateMap<UpdateVaccinationDto, Vaccination>().ReverseMap();
        }
    }
}
