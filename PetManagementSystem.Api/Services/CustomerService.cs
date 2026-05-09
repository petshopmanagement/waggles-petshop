using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Exceptions;

namespace PetManagementSystem.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<CustomerDto>>(await _repo.GetAllAsync());

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) throw new CustomerNotFoundException($"Customer with ID {id} not found.");
        return _mapper.Map<CustomerDto>(c);
    }

    public async Task<CustomerProfileDto?> GetProfileAsync(int id)
    {
        var c = await _repo.GetWithAddressAsync(id);
        if (c == null) throw new CustomerNotFoundException($"Customer with ID {id} not found.");
        return _mapper.Map<CustomerProfileDto>(c);
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(int customerId)
        => _mapper.Map<IEnumerable<TransactionDto>>(await _repo.GetTransactionsAsync(customerId));

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = _mapper.Map<Customer>(dto);
        var updated = await _repo.UpdateAsync(id, customer);
        return updated == null ? null : _mapper.Map<CustomerDto>(updated);
    }

    public async Task<CustomerDto?> PatchAsync(int id, PatchCustomerDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new CustomerNotFoundException($"Customer with ID {id} not found.");

        if (dto.FirstName != null) existing.FirstName = dto.FirstName;
        if (dto.LastName != null) existing.LastName = dto.LastName;
        if (dto.Email != null) existing.Email = dto.Email;
        if (dto.PhoneNumber != null) existing.PhoneNumber = dto.PhoneNumber;
        if (dto.AddressId.HasValue) existing.AddressId = dto.AddressId;

        var updated = await _repo.UpdateAsync(id, existing);
        return updated == null ? null : _mapper.Map<CustomerDto>(updated);
    }

    public async Task<CustomerProfileDto?> AddAddressAsync(int id, AddressDto addressDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new CustomerNotFoundException($"Customer with ID {id} not found.");

        existing.Address = _mapper.Map<Address>(addressDto);
        var updated = await _repo.UpdateAsync(id, existing);

        // Return the full profile so they see the new address attached!
        return updated == null ? null : await GetProfileAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
        => await _repo.DeleteAsync(id);
}
