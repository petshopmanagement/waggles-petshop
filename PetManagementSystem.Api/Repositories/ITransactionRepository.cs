using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<Transaction?> GetByIdAsync(int id);
    Task<IEnumerable<Transaction>> GetByCustomerAsync(int customerId, int page = 1, int pageSize = 10);
    Task<IEnumerable<Transaction>> GetByPetAsync(int petId);
    Task<decimal> GetTotalRevenueAsync();
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<Transaction?> UpdateStatusAsync(int id, string status);
    Task<IEnumerable<Transaction>> SearchAsync(string query, string? status, int page = 1, int pageSize = 10);
}
