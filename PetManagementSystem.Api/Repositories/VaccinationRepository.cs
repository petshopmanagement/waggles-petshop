using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public class VaccinationRepository : IVaccinationRepository
    {
        private readonly PetStoreDbContext _context;

        public VaccinationRepository(PetStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vaccination>> GetAllAsync()
        {
            return await _context.Vaccinations.ToListAsync();
        }

        public async Task<Vaccination?> GetByIdAsync(int id)
        {
            return await _context.Vaccinations.FirstOrDefaultAsync(v => v.VaccinationId == id);
        }

        public async Task<Vaccination> AddAsync(Vaccination vaccination)
        {
            await _context.Vaccinations.AddAsync(vaccination);
            await _context.SaveChangesAsync();
            return vaccination;
        }

        public async Task<Vaccination?> UpdateAsync(int id, Vaccination vaccination)
        {
            var existing = await _context.Vaccinations.FindAsync(id);

            if (existing == null)
                return null;

            existing.Name = vaccination.Name;
            existing.Description = vaccination.Description;
            existing.Price = vaccination.Price;
            existing.Available = vaccination.Available;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<IEnumerable<Pet>> GetAllPetsAsync(int id)
        {
            var vaccination = await _context.Vaccinations.Include(v => v.Pets).FirstOrDefaultAsync(v => v.VaccinationId == id);
            return vaccination?.Pets.ToList() ?? new List<Pet>();
        }
    }
}
