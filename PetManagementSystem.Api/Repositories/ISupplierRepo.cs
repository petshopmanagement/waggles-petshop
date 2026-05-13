using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public interface ISupplierRepo
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task<IEnumerable<Pet>> GetAllPetsAsync(int id);
        Task<IEnumerable<Pet>> SearchPetsAsync(int id, string query, int? categoryId);
    }
}
