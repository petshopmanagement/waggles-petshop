using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);
    Task<CustomerDto> GetProfileAsync(int id);
    Task<IEnumerable<TransactionDto>> GetTransactionsAsync(int customerId);
    Task<CustomerDto> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<CustomerDto> PatchAsync(int id, UpdateCustomerDto dto);
    Task<CustomerDto> AddAddressAsync(int id, AddressDto addressDto);

}