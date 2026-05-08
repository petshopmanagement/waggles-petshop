using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace PetManagementSystem.Api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PetStoreDbContext _context;
        public EmployeeRepository(PetStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.Include(e => e.Address).ToListAsync();
        }

        public async Task<Employee?> GetEmpByIdAsync(int id)
        {
            return await _context.Employees.Include(e => e.Address).FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee?> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
        public async Task<IEnumerable<Pet>> GetEmployeeWithPetsAsync(int id)
        {
            //return await _context.Employees.Include(e => e.Pets).FirstOrDefaultAsync(e => e.EmployeeId == id);

            var emp = await _context.Employees
                            .Include(s => s.Pets)
                            .FirstOrDefaultAsync(s => s.EmployeeId == id);

            return emp?.Pets.ToList();

        }


    }
}
