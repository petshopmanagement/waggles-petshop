using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;

namespace PetManagementSystem.Api.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupplierService> _logger;
        public SupplierService(ISupplierRepo repository, IMapper mapper, ILogger<SupplierService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            var suppliers = await _repository.GetAllAsync();
            if (suppliers == null)
                throw new DataNotFoundException($"No Suppliers found");
            return _mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }
        public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);
            if (supplier == null) throw new SupplierNotFoundException("Supplier not found");
            return supplier == null ? null : _mapper.Map<SupplierDTO>(supplier);
        }
        public async Task<SupplierDTO> CreateSupplierAsync(SupplierDTO dto)
        {
            var supplierEntity = _mapper.Map<Supplier>(dto);
            var createdSupplier = await _repository.AddAsync(supplierEntity);
            _logger.LogInformation("Created new supplier: {SupplierName}", createdSupplier.Name);

            // Re-fetch to include navigation properties (like Address)
            var completeSupplier = await _repository.GetByIdAsync(createdSupplier.SupplierId);
            return _mapper.Map<SupplierDTO>(completeSupplier);
        }

        public async Task PatchSupplierAsync(int id, JsonPatchDocument<SupplierDTO> patchDoc)
        {
            var existingSupplier = await _repository.GetByIdAsync(id);
            if (existingSupplier == null) throw new SupplierNotFoundException("Supplier not found");

            var dto = _mapper.Map<SupplierDTO>(existingSupplier);
            patchDoc.ApplyTo(dto);

            _mapper.Map(dto, existingSupplier);
            await _repository.UpdateAsync(existingSupplier);
            _logger.LogInformation("Patched supplier with ID: {SupplierId}", id);
        }

        public async Task<IEnumerable<PetDto>> GetPetsAsync(int id)
        {
            var pets = await _repository.GetAllPetsAsync(id);
            if (pets == null || !pets.Any())
                throw new DataNotFoundException($"No Pets are Linked to Supplier with id: {id}");

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        
        
    }
}