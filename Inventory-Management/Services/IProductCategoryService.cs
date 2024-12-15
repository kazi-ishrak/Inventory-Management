using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Services
{
    public interface IProductCategoryService
    {
        public Task<List<ProductCategory>?> GetAll();
    }
}
