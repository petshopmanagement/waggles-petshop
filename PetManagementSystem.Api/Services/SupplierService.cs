using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public SupplierService(ISupplierRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
            return _mapper.Map<SupplierDto>(createdSupplier);
        }
        public async Task UpdateSupplierAsync(int id, UpdateSupplierDto dto)
        {
            var existingSupplier = await _repository.GetByIdAsync(id);
            if (existingSupplier == null) throw new SupplierNotFoundException("Supplier not found");

            _mapper.Map(dto, existingSupplier);

            await _repository.UpdateAsync(existingSupplier);
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
