using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public class GroomingServiceRepo : IGroomingServiceRepo
    {
        private readonly PetStoreDbContext _context;

        public GroomingServiceRepo(PetStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroomingService>> GetAllAsync()
        {
            return await _context.GroomingServices.ToListAsync();
        }

        public async Task<GroomingService?> GetByIdAsync(int id)
        {
            return await _context.GroomingServices.FirstOrDefaultAsync(s => s.ServiceId == id);
        }

        public async Task<GroomingService> AddAsync(GroomingService service)
        {
            await _context.GroomingServices.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task UpdateAsync(GroomingService service)
        {
            _context.Entry(service).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Pet>> GetAllPetsAsync(int id)
        {
            var service = await _context.GroomingServices
                .Include(s => s.Pets)
                .FirstOrDefaultAsync(s => s.ServiceId == id);

            return service?.Pets.ToList() ?? new List<Pet>();
        }
    }
}
