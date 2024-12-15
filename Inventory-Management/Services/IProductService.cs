using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Services
{
    public interface IProductService
    {
        Task Create(Product input);
        Task<List<Product>> GetAll();
        Task<Product> GetById(long id);
        Task Delete(long id);
        Task Update(Product input);
    }
}
