using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.DTOs.SupplierDtos;
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
        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _repository.GetAllAsync();
            if (suppliers== null)
                throw new DataNotFoundException($"No Suppliers found");
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }
        public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);
            if (supplier == null) throw new SupplierNotFoundException("Supplier not found");
            return supplier == null ? null : _mapper.Map<SupplierDto>(supplier);
        }
        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
        {
            var supplierEntity = _mapper.Map<Supplier>(dto);
            var createdSupplier = await _repository.AddAsync(supplierEntity);
            _logger.LogInformation("Created new supplier: {SupplierName}", createdSupplier.Name);
            return _mapper.Map<SupplierDto>(createdSupplier);
        }

        public async Task PatchSupplierAsync(int id, JsonPatchDocument<UpdateSupplierDto> patchDoc)
        {
            var existingSupplier = await _repository.GetByIdAsync(id);
            if (existingSupplier == null) throw new SupplierNotFoundException("Supplier not found");

            var dto = _mapper.Map<UpdateSupplierDto>(existingSupplier);
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
