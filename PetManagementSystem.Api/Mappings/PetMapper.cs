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

            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();


            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionDto, Transaction>();

            CreateMap<GroomingService, GroomingDTO>();
            CreateMap<GroomingDTO, GroomingService>();
            CreateMap<Vaccination, VaccinationDTO>();
            CreateMap<VaccinationDTO, Vaccination>();
        }
    }
}
