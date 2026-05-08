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
                throw new NotFoundException("Employee not found");
            }
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _repository.GetEmpByIdAsync(id);
            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }
            _mapper.Map(dto, employee);
            var updatedEmp = await _repository.UpdateAsync(employee);
            return _mapper.Map<EmployeeDto>(updatedEmp);
        }

        public async Task<IEnumerable<PetDto>> GetPetsByEmpIdAsync(int id)
        {
            var pets = await _repository.GetEmployeeWithPetsAsync(id);
            if (pets == null || !pets.Any())
                throw new NotFoundException($"No Pets are Linked to Employee with id: {id}");

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }
    }
}
