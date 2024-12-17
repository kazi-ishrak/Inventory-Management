using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Services
{
    public interface IProductCategoryService
    {
        Task Create(ProductCategory input);
        Task<List<ProductCategory>> GetAll();
        Task<List<ProductCategory>> GetAllByCategory(int categoryId);
        Task<List<ProductCategory>> GetAllByProduct(int productId);
        Task<ProductCategory> GetById(long id);
        Task Delete(long id);
        Task Update(ProductCategory input);
    }
}
