using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public interface IGroomingServiceRepo
    {
        Task<IEnumerable<GroomingService>> GetAllAsync();
        Task<GroomingService?> GetByIdAsync(int id);
        Task<GroomingService> AddAsync(GroomingService service);
        Task UpdateAsync(GroomingService service);
        Task<IEnumerable<Pet>> GetAllPetsAsync(int id);
    }
}
