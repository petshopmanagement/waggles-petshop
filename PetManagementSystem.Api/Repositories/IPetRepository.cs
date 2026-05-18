using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public interface IPetRepository
    {

        Task<IEnumerable<Pet>> GetAllPets(int page = 1, int pageSize = 10);

        Task<Pet?> GetPetById(int petid);

        Task<IEnumerable<Pet>> GetPetByCategory(int categoryId, int page = 1, int pageSize = 10);

        Task<IEnumerable<Pet>> GetPetByName(string name, int page = 1, int pageSize = 10);

        Task AddPet(Pet pet);

        Task UpdatePet(Pet pet);

        Task<IEnumerable<Supplier>> GetSuppliersByPetId(int petId);
        Task<IEnumerable<Employee>> GetEmployeeById(int petId);

        Task<IEnumerable<Transaction>> GetTransactionByPetId(int petId);

        Task<IEnumerable<GroomingService>> GetGroomingsByPetId(int petId);

        Task<IEnumerable<Vaccination>> GetVaccinationByPetId(int petId);
    }
}
