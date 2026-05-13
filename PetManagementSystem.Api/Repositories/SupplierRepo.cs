using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;
using System.Linq;

namespace PetManagementSystem.Api.Repositories
{
    public class SupplierRepo : ISupplierRepo
    {
        private readonly PetStoreDbContext _context;
        public SupplierRepo(PetStoreDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.Include(s => s.Address).Include(s => s.Pets).ToListAsync();
        }
        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.Include(s => s.Address).Include(s => s.Pets).FirstOrDefaultAsync(s => s.SupplierId == id);
        }
        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }
        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Entry(supplier).State = EntityState.Modified;
            // _context.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Pet>> GetAllPetsAsync(int id)
        {
            var supplier = await _context.Suppliers
             .Include(s => s.Pets)
             .FirstOrDefaultAsync(s => s.SupplierId == id);

            return supplier?.Pets.ToList() ?? new List<Pet>();
        }

        public async Task<IEnumerable<Pet>> SearchPetsAsync(int id, string query, int? categoryId)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Pets)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier == null) return new List<Pet>();

            var pets = supplier.Pets.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                pets = pets.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                                       p.Breed.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                pets = pets.Where(p => p.CategoryId == categoryId);
            }

            return pets.ToList();
        }

        
    }
}