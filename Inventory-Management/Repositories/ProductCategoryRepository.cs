using Inventory_Management.Data;
using Inventory_Management.Services;
using Microsoft.EntityFrameworkCore;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Repositories
{
    public class ProductCategoryRepository : IProductCategoryService
    {
        private readonly ApplicationDbContext _db;

        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task Create(ProductCategory input)
        {
            _db.ProductCategories.Add(input);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> GetAll()
        {
            return await _db.ProductCategories.ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAllByCategory(int categoryId)
        {
            return await _db.ProductCategories.Where(i=> i.CategoryId == categoryId).ToListAsync();
        }

        public async Task<ProductCategory> GetById(long id)
        {
            return await _db.ProductCategories
                .Where(pc => pc.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(long id)
        {
            ProductCategory data = await GetById(id);
            if (data != null)
            {
                _db.ProductCategories.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(ProductCategory input)
        {
            if (input != null)
            {
                _db.ProductCategories.Update(input);
                await _db.SaveChangesAsync();
            }
        }
    }
}
