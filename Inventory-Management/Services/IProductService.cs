using Inventory_Management.Models;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Services
{
    public interface IProductService
    {
        Task<Product?> Create(Product input);
        Task<List<ProductDto>> GetAll();
        Task<Product> GetById(long id);
        Task Delete(long id);
        Task Update(Product input);
    }
}
