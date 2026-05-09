using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class PetMapper : Profile
    {
        public PetMapper()
        {

            CreateMap<PetCreate, Pet>();
            CreateMap<PetUpdate, Pet>();
            CreateMap<SupplierDTO, Supplier>();
            CreateMap<Supplier, SupplierDTO>();
            CreateMap<Pet, PetDTO>();

            CreateMap<CustomerDTO, Customer>();
            CreateMap<Customer, CustomerDTO>();

            CreateMap<Employee, EmployeeDTO>();
            CreateMap<Customer, CustomerDTO>();
        }
    }
}
