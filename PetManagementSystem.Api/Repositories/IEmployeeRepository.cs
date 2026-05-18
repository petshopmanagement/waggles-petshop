using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetEmpByIdAsync(int id);
        Task<Employee?> CreateAsync(Employee employee);
        Task<Employee?> UpdateAsync(int id, Employee employee);
        Task<IEnumerable<Pet>> GetEmployeeWithPetsAsync(int id);
        Task<Employee?> GetByEmailAsync(string email);
    }
}
