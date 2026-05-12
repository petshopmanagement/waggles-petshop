using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly PetStoreDbContext _context;

    public CustomerRepository(PetStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
        => await _context.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<Customer?> GetWithAddressAsync(int id)
        => await _context.Customers.Include(c => c.Address)
        .FirstOrDefaultAsync(c => c.CustomerId == id);

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(int customerId)
        => await _context.Transactions
            .Include(t => t.Pet)
            .Where(t => t.CustomerId == customerId)
            .ToListAsync();

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer?> UpdateAsync(int id, Customer customer)
    {
        var existing = await _context.Customers.FindAsync(id);
        if (existing == null) return null;
        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Email = customer.Email;
        existing.PhoneNumber = customer.PhoneNumber;
        existing.AddressId = customer.AddressId;
        await _context.SaveChangesAsync();
        return existing;
    }



}