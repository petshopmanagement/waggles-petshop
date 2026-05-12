using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> GetEmpByIdAsync(int id);
        Task<EmployeeDto> CreateEmployeeAsync(WriteEmployeeDto dto);
        Task<EmployeeDto> UpdateEmployeeAsync(int id, WriteEmployeeDto dto);
        Task<EmployeeDto?> PatchEmployeeAsync(int id, WriteEmployeeDto dto);
        Task<IEnumerable<PetDto>> GetPetsByEmpIdAsync(int id); 
    }
}
