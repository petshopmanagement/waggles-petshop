using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<TransactionDto> GetByIdAsync(int id);               // throws TransactionNotFoundException
    Task<IEnumerable<TransactionDto>> GetByCustomerAsync(int customerId, int page = 1, int pageSize = 10);
    Task<IEnumerable<TransactionDto>> GetByPetAsync(int petId);
    Task<decimal> GetTotalRevenueAsync();
    Task<SalesSummaryDto> GetSalesSummaryAsync();
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
    Task<TransactionDto> UpdateStatusAsync(int id, UpdateTransactionStatusDto dto); // throws if not found
    Task<IEnumerable<TransactionDto>> SearchAsync(string query, string? status, int page = 1, int pageSize = 10);
}
