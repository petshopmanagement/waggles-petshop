using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Exceptions;
namespace PetManagementSystem.Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            var createdEmployee = await _repository.CreateAsync(employee);
            return _mapper.Map<EmployeeDto>(createdEmployee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> GetEmpByIdAsync(int id)
        {
            var employee = await _repository.GetEmpByIdAsync(id);
            if (employee == null)
            {
                throw new EmployeeNotFoundException("Employee not found");
            }
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _repository.GetEmpByIdAsync(id);

            if (employee == null)
                throw new EmployeeNotFoundException("Employee not found");

            // Full update using AutoMapper
            _mapper.Map(dto, employee);

            var updatedEmployee = await _repository.UpdateAsync(id, employee);

            if (updatedEmployee == null)
                throw new EmployeeNotFoundException("Employee not found");

            return _mapper.Map<EmployeeDto>(updatedEmployee);
        }
        public async Task<EmployeeDto?> PatchEmployeeAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _repository.GetEmpByIdAsync(id);

            if (employee == null)
                throw new EmployeeNotFoundException("Employee not found");

            if (dto.FirstName != null)
                employee.FirstName = dto.FirstName;

            if (dto.LastName != null)
                employee.LastName = dto.LastName;

            if (dto.Position != null)
                employee.Position = dto.Position;

            if (dto.HireDate.HasValue)
                employee.HireDate = dto.HireDate.Value;

            if (dto.PhoneNumber != null)
                employee.PhoneNumber = dto.PhoneNumber;

            if (dto.Email != null)
                employee.Email = dto.Email;

            if (dto.AddressId.HasValue)
                employee.AddressId = dto.AddressId.Value;

            var updatedEmployee = await _repository.UpdateAsync(id, employee);

            return updatedEmployee == null
                ? null
                : _mapper.Map<EmployeeDto>(updatedEmployee);
        }
        public async Task<IEnumerable<PetDto>> GetPetsByEmpIdAsync(int id)
        {
            var pets = await _repository.GetEmployeeWithPetsAsync(id);
            if (pets == null || !pets.Any())
                throw new EmployeeNotFoundException($"No Pets are Linked to Employee with id: {id}");

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }
    }
}
