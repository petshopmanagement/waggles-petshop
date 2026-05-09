using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public interface IVaccinationRepository
    {
        Task<IEnumerable<Vaccination>> GetAllAsync();
        Task<Vaccination?> GetByIdAsync(int id);
        Task<Vaccination> AddAsync(Vaccination vaccination);
        Task<Vaccination?> UpdateAsync(int id, Vaccination vaccination);
        Task<IEnumerable<Pet>> GetAllPetsAsync(int id);
    }
}
