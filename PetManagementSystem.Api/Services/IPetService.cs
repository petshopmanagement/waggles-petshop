using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Services
{
    public interface IPetService
    {
        Task<IEnumerable<PetDto>> GetAllPets();

        Task<PetDto?> GetPetById(int petid);

        Task<IEnumerable<PetDto>> GetPetByCategory(int categoryId);

        Task<IEnumerable<PetDto>> GetPetByName(string name);

        Task<PetDto> AddPet(PetCreate dto);

        Task UpdatePet(int petid, PetUpdate dto);

        Task<IEnumerable<SupplierDTO>> GetSuppliersByPetIdService(int petId);
        Task<IEnumerable<EmployeeDto>> GetEmployeeByPetIdService(int petId);

        Task<IEnumerable<TransactionDto>> GetTrasactionbypetID(int petId);

        Task<IEnumerable<GroomingDTO>> GetGroomingsByPetId(int petId);

        Task<IEnumerable<VaccinationDto>> GetVaccinationByPetId(int petId);
    }
}
