using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Customer, CustomerProfileDto>()
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address));
            //CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();
             CreateMap<Address, AddressDto>();
           
            CreateMap<CreateAddressDto, Address>();
            CreateMap<CreateAddressDto, Address>();
            CreateMap<Transaction, TransactionDto>();

        }
    }
}
