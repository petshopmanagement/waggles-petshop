using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public interface IPetRepository
    {

        Task<IEnumerable<Pet>> GetAllPets();

        Task<Pet?> GetPetById(int petid);

        Task<IEnumerable<Pet>> GetPetByCategory(int categoryId);

        Task<IEnumerable<Pet>> GetPetByName(string name);

        Task AddPet(Pet pet);

        Task UpdatePet(Pet pet);

        Task<IEnumerable<Supplier>> GetSuppliersByPetId(int petId);
        Task<IEnumerable<Employee>> GetEmployeeById(int petId);
    }
}
