using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> GetEmpByIdAsync(int id);
        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
        Task<EmployeeDto?> PatchEmployeeAsync(int id, UpdateEmployeeDto dto);
        Task<IEnumerable<PetDto>> GetPetsByEmpIdAsync(int id); 
    }
}
