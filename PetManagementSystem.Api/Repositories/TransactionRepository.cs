using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly PetStoreDbContext _context;

    public TransactionRepository(PetStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(int page = 1, int pageSize = 10)
        => await _context.Transactions
            .Include(t => t.Customer)
            .Include(t => t.Pet)
            .OrderByDescending(t => t.TransactionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<Transaction?> GetByIdAsync(int id)
        => await _context.Transactions.Include(t => t.Customer).Include(t => t.Pet)
            .FirstOrDefaultAsync(t => t.TransactionId == id);

    public async Task<IEnumerable<Transaction>> GetByCustomerAsync(int customerId, int page = 1, int pageSize = 10)
        => await _context.Transactions
            .Include(t => t.Pet)
            .Where(t => t.CustomerId == customerId)
            .OrderByDescending(t => t.TransactionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<Transaction>> GetByPetAsync(int petId)
        => await _context.Transactions.Include(t => t.Customer)
            .Where(t => t.PetId == petId).ToListAsync();

    public async Task<decimal> GetTotalRevenueAsync()
        => await _context.Transactions
            .Where(t => t.TransactionStatus == "Success")
            .SumAsync(t => t.Amount ?? 0);

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction?> UpdateStatusAsync(int id, string status)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return null;
        transaction.TransactionStatus = status;
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<IEnumerable<Transaction>> SearchAsync(string query, string? status, int page = 1, int pageSize = 10)
    {
        var transactions = _context.Transactions
            .Include(t => t.Customer)
            .Include(t => t.Pet)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            transactions = transactions.Where(t => 
                t.Customer.FirstName.Contains(query) || 
                t.Customer.LastName.Contains(query) || 
                t.Pet.Name.Contains(query) ||
                t.TransactionId.ToString() == query);
        }

        if (!string.IsNullOrEmpty(status) && status != "All")
        {
            transactions = transactions.Where(t => t.TransactionStatus == status);
        }

        return await transactions
            .OrderByDescending(t => t.TransactionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
