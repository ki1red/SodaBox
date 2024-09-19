using SodaBox.DataAccess.Entities;

namespace SodaBox.DataAccess.IRepositories
{
    public interface IBrandRepository
    {
        Task<Brand> GetBrandByIdAsync(int id);
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
    }
}
