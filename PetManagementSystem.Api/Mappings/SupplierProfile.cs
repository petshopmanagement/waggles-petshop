using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class SupplierProfile:Profile
    {
        public SupplierProfile()
        {
            
            CreateMap<Supplier, SupplierDto>()
                .ReverseMap();
            CreateMap<Pet, PetDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
