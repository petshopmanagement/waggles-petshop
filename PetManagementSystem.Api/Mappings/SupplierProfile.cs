using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.DTOs.SupplierDtos;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class SupplierProfile:Profile
    {
        public SupplierProfile()
        {
            
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Pet, PetDto>().ReverseMap();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>().ReverseMap();
        }
    }
}
