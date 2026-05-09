using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(int id);
    Task<IEnumerable<Transaction>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<Transaction>> GetByPetAsync(int petId);
    Task<decimal> GetTotalRevenueAsync();
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<Transaction?> UpdateStatusAsync(int id, string status);
}
