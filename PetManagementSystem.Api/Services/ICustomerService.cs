using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);
    Task<CustomerProfileDto> GetProfileAsync(int id);
    Task<IEnumerable<TransactionDto>> GetTransactionsAsync(int customerId);
    Task<CustomerDto> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<CustomerDto> PatchAsync(int id, PatchCustomerDto dto);
    Task<CustomerProfileDto> AddAddressAsync(int id, AddressDto addressDto);
    Task DeleteAsync(int id);
}