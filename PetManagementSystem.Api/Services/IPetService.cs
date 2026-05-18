using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Services
{
    public interface IPetService
    {
        Task<IEnumerable<PetDto>> GetAllPets(int page = 1, int pageSize = 10);
        Task<int> GetTotalPetCount();

        Task<PetDto?> GetPetById(int petid);

        Task<IEnumerable<PetDto>> GetPetByCategory(int categoryId, int page = 1, int pageSize = 10);

        Task<IEnumerable<PetDto>> GetPetByName(string name, int page = 1, int pageSize = 10);

        Task<PetDto> AddPet(PetCreate dto);

        Task UpdatePet(int petid, PetUpdate dto);

        Task<IEnumerable<SupplierDTO>> GetSuppliersByPetIdService(int petId);
        Task<IEnumerable<EmployeeDto>> GetEmployeeByPetIdService(int petId);

        Task<IEnumerable<TransactionDto>> GetTrasactionbypetID(int petId);

        Task<IEnumerable<GroomingDTO>> GetGroomingsByPetId(int petId);

        Task<IEnumerable<VaccinationDto>> GetVaccinationByPetId(int petId);

    }
}
