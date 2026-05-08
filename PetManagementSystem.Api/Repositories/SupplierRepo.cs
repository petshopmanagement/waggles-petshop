using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;
using System.Linq;

namespace PetManagementSystem.Api.Repositories
{
    public class SupplierRepo:ISupplierRepo
    {
        private readonly PetStoreDbContext _context;
        public SupplierRepo(PetStoreDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.Include(s => s.Address).ToListAsync();
        }
        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.Include(s => s.Address).FirstOrDefaultAsync(s => s.SupplierId == id);
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

            return supplier?.Pets.ToList();
        }
        //public async Task<Supplier> GetByPetIdAsync(int id)
        //{
        //    return await _context.Suppliers.FirstOrDefaultAsync
        //        (x => x.Pets.Any(x => x.PetId == id));
        //}
    }
}
