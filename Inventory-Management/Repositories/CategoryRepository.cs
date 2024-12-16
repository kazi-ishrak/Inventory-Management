using Inventory_Management.Data;
using Inventory_Management.Services;
using Microsoft.EntityFrameworkCore;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Repositories
{
    public class CategoryRepository : ICategoryService
    {
        private readonly ApplicationDbContext _db;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;

        public CategoryRepository(ApplicationDbContext context, IProductCategoryService productCategoryService, IProductService productService)
        {
            _db = context;
            _productCategoryService = productCategoryService;
            _productService = productService;
        }

        public async Task Create(Category input)
        {
            input.Created_at = DateTime.Now;
            input.Updated_at = DateTime.Now;
            _db.Categories.Add(input);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAll()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> GetById(long id)
        {
            return await _db.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(int id)
        {
            Category data = await GetById(id);
            if (data == null)
            {
                return;
            }

            _db.Categories.Remove(data);
            await _db.SaveChangesAsync();

            var productCategories = await _productCategoryService.GetAllByCategory(id);
            if (productCategories != null)
            {
                var productIds = productCategories.Select(i => i.ProductId).ToList();
                foreach (var productId in productIds)
                {
                    var product = await _productService.GetById(productId);
                    if (product != null)
                    {
                        await _productService.Delete(productId);
                    }
                }
            }
        }

        public async Task Update(Category input)
        {
            var Data = await _db.Categories.Where(x => x.Id == input.Id).FirstOrDefaultAsync();
            try
            {
                if (Data != null)
                {
                    Data.Name = input.Name;
                    Data.Updated_at = DateTime.Now;

                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }

        }


    }
}
