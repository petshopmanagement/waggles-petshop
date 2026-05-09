using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Services
{
    public interface IPetService
    {
        Task<IEnumerable<PetDTO>> GetAllPets();

        Task<PetDTO?> GetPetById(int petid);

        Task<IEnumerable<PetDTO>> GetPetByCategory(int categoryId);

        Task<IEnumerable<PetDTO>> GetPetByName(string name);

        Task<PetDTO> AddPet(PetCreate dto);

        Task UpdatePet(int petid, PetUpdate dto);

        Task<IEnumerable<SupplierDTO>> GetSuppliersByPetIdService(int petId);
        Task<IEnumerable<EmployeeDTO>> GetEmployeeByPetIdService(int petId);
    }
}
