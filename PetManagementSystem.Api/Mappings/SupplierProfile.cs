using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class SupplierProfile:Profile
    {
        public SupplierProfile()
        {
            
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Pet, PetDto>();
            CreateMap<PetDto, Pet>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();
        }
    }
}
