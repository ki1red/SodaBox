using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess.Entities;
using SodaBox.DataAccess.IRepositories;

namespace SodaBox.DataAccess.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly VendingMachineContext _context;
        public BrandRepository(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _context.brands.ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.brands.FindAsync(id);
        }
    }
}
