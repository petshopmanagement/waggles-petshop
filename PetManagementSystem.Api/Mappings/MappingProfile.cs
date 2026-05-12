using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<WriteEmployeeDto, Employee>();
            CreateMap<WriteEmployeeDto, Employee>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<UpdateCustomerDto, Customer>();
             CreateMap<Address, AddressDto>();
            CreateMap<Pet, PetDto>();
           
            CreateMap<WriteAddressDto, Address>();
            CreateMap<WriteAddressDto, Address>();
            CreateMap<Transaction, TransactionDto>();

        }
    }
}
