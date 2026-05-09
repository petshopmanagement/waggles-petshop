using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.DTOs.GroomingServiceDtos;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;

namespace PetManagementSystem.Api.Services
{
    public class GroomingServiceService : IGroomingServiceService
    {
        private readonly IGroomingServiceRepo _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GroomingServiceService> _logger;

        public GroomingServiceService(IGroomingServiceRepo repo, IMapper mapper, ILogger<GroomingServiceService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<GroomingServiceDto>> GetAllAsync()
        {
            var services = await _repo.GetAllAsync();
            if (services == null || !services.Any())
                throw new DataNotFoundException("No grooming services found.");
            
            return _mapper.Map<IEnumerable<GroomingServiceDto>>(services);
        }

        public async Task<GroomingServiceDto?> GetByIdAsync(int id)
        {
            var service = await _repo.GetByIdAsync(id);
            if (service == null)
                throw new DataNotFoundException($"Grooming service with ID {id} not found.");
            
            return _mapper.Map<GroomingServiceDto>(service);
        }

        public async Task<GroomingServiceDto> CreateAsync(CreateGroomingServiceDto dto)
        {
            var service = _mapper.Map<GroomingService>(dto);
            var createdService = await _repo.AddAsync(service);
            _logger.LogInformation("Created new grooming service: {ServiceName}", createdService.Name);
            return _mapper.Map<GroomingServiceDto>(createdService);
        }
        public async Task PatchAsync(int id, JsonPatchDocument<UpdateGroomingServiceDto> patchDoc)
        {
            var existingService = await _repo.GetByIdAsync(id);
            if (existingService == null)
                throw new DataNotFoundException($"Grooming service with ID {id} not found.");

            var dto = _mapper.Map<UpdateGroomingServiceDto>(existingService);
            patchDoc.ApplyTo(dto);

            _mapper.Map(dto, existingService);
            await _repo.UpdateAsync(existingService);
            _logger.LogInformation("Patched grooming service with ID: {ServiceId}", id);
        }


        public async Task<IEnumerable<PetDto>> GetPetsAsync(int id)
        {
            var pets = await _repo.GetAllPetsAsync(id);
            if (pets == null || !pets.Any())
                throw new DataNotFoundException($"No pets found for grooming service with ID {id}.");

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }
    }
}
