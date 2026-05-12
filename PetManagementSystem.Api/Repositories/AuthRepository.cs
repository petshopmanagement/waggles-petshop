using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly PetStoreDbContext _context;

    public AuthRepository(PetStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
        => await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        => await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

    public async Task<Supplier?> GetSupplierByEmailAsync(string email)
        => await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == email);

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
    }
}
