using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Exceptions;

namespace PetManagementSystem.Api.Services
{
    public class VaccinationService : IVaccinationService
    {
        private readonly IVaccinationRepository _repo;
        private readonly IMapper _mapper;

        public VaccinationService(IVaccinationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VaccinationDto>> GetAllAsync()
        {
            var vaccinations = await _repo.GetAllAsync();

            if (vaccinations == null || !vaccinations.Any())
                throw new EmployeeNotFoundException("No vaccinations found.");

            return _mapper.Map<IEnumerable<VaccinationDto>>(vaccinations);
        }

        public async Task<VaccinationDto?> GetByIdAsync(int id)
        {
            var vaccination = await _repo.GetByIdAsync(id);

            if (vaccination == null)
                throw new EmployeeNotFoundException($"Vaccination with ID {id} not found.");

            return _mapper.Map<VaccinationDto>(vaccination);
        }

        public async Task<VaccinationDto> CreateAsync(WriteVaccinationDto dto)
        {
            var vaccination = _mapper.Map<Vaccination>(dto);
            var createdVaccination = await _repo.AddAsync(vaccination);

            return _mapper.Map<VaccinationDto>(createdVaccination);
        }

        public async Task<VaccinationDto> UpdateAsync(int id, WriteVaccinationDto dto)
        {
            var vaccination = await _repo.GetByIdAsync(id);

            if (vaccination == null)
                throw new EmployeeNotFoundException($"Vaccination with ID {id} not found.");

            _mapper.Map(dto, vaccination);

            var updatedVaccination = await _repo.UpdateAsync(id, vaccination);

            if (updatedVaccination == null)
                throw new EmployeeNotFoundException($"Vaccination with ID {id} not found.");

            return _mapper.Map<VaccinationDto>(updatedVaccination);
        }

        public async Task<VaccinationDto?> PatchAsync(int id, WriteVaccinationDto dto)
        {
            var vaccination = await _repo.GetByIdAsync(id);

            if (vaccination == null)
                throw new EmployeeNotFoundException($"Vaccination with ID {id} not found.");

            if (dto.Name != null)
                vaccination.Name = dto.Name;

            if (dto.Description != null)
                vaccination.Description = dto.Description;

            if (dto.Price.HasValue)
                vaccination.Price = dto.Price.Value;

            if (dto.Available.HasValue)
                vaccination.Available = dto.Available.Value;

            var updatedVaccination = await _repo.UpdateAsync(id, vaccination);

            return updatedVaccination == null
                ? null
                : _mapper.Map<VaccinationDto>(updatedVaccination);
        }

        public async Task<IEnumerable<PetDto>> GetPetsAsync(int id)
        {
            var pets = await _repo.GetAllPetsAsync(id);

            if (pets == null || !pets.Any())
                throw new EmployeeNotFoundException($"No pets found for vaccination with ID {id}.");

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }
    }
}