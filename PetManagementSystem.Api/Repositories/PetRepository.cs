using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly PetStoreDbContext _context;

        public PetRepository(PetStoreDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Pet>> GetAllPets(int page = 1, int pageSize = 10)
        {
            return await _context.Pets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetByCategory(int categoryId, int page = 1, int pageSize = 10)
        {
            return await _context.Pets
                .Where(p => p.CategoryId == categoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Pet?> GetPetById(int petid)
        {
            return await _context.Pets.FindAsync(petid);
        }

        public async Task<IEnumerable<Pet>> GetPetByName(string name, int page = 1, int pageSize = 10)
        {
            return await _context.Pets
                .Where(p => p.Name.Contains(name))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddPet(Pet pet)
        {
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

        }


        public async Task UpdatePet(Pet pet)
        {
            _context.Entry(pet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByPetId(int petId)
        {
            var pet = await _context.Pets
                .Include(p => p.Suppliers)
                .FirstOrDefaultAsync(p => p.PetId == petId);
            return pet?.Suppliers.ToList();
        }

        public async Task<IEnumerable<Employee>> GetEmployeeById(int petId)
        {
            var pet = await _context.Pets
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.PetId == petId);
            return pet?.Employees.ToList();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionByPetId(int petId)
        {
            return await _context.Transactions
                         .Include(x => x.Customer)
                         .Include(x => x.Pet)
                         .Where(x => x.PetId == petId)
                         .ToListAsync();
        }

        public async Task<IEnumerable<GroomingService>> GetGroomingsByPetId(int petId)
        {
            var pet = await _context.Pets
               .Include(x => x.Services)
               .FirstOrDefaultAsync(x => x.PetId == petId);

            if (pet == null)
            {
                return Enumerable.Empty<GroomingService>();
            }

            return pet.Services;
        }

        public  async Task<IEnumerable<Vaccination>> GetVaccinationByPetId(int petId)
        {
            var pet = await _context.Pets
              .Include(x => x.Vaccinations)
              .FirstOrDefaultAsync(x => x.PetId == petId);

            if (pet == null)
            {
                return Enumerable.Empty<Vaccination>();
            }

            return pet.Vaccinations;
        }
    }
}
