using Inventory_Management.Data;
using Inventory_Management.Handler;
using Inventory_Management.Services;
using static Inventory_Management.Models.DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Repositories
{
    public class ProductCategoryRepository : IProductCategoryService
    {
        private readonly ApplicationDbContext _db;
        public ProductCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        public async Task<List<ProductCategory>?> GetAll()
        {
            try
            {
                return await _db.ProductCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return null;
            }
        }
    }
}
