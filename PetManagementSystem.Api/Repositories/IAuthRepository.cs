using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories;

public interface IAuthRepository
{
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<Employee?> GetEmployeeByEmailAsync(string email);
    Task<Supplier?> GetSupplierByEmailAsync(string email);

    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<Supplier> CreateSupplierAsync(Supplier supplier);

    Task UpdateCustomerAsync(Customer customer);
    Task UpdateEmployeeAsync(Employee employee);
    Task UpdateSupplierAsync(Supplier supplier);
}