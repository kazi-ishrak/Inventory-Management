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
            try
            {
                input.Created_at = DateTime.Now;
                input.Updated_at = DateTime.Now;
                _db.Categories.Add(input);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            
        }

        public async Task<List<Category>?> GetAll()
        {
            try
            {
                return await _db.Categories.ToListAsync();
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }

        public async Task<Category?> GetById(long id)
        {
            try
            {
                return await _db.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task Delete(int id)
        {

            Category? data = await GetById(id);
            if (data == null)
            {
                return; 
            }

            var executionStrategy = _db.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.Categories.Remove(data);
                        await _db.SaveChangesAsync();

                        var productCategories = await _productCategoryService.GetAllByCategory(id);
                        if (productCategories != null)
                        {
                            foreach (var productCategory in productCategories)
                            {
                                _db.ProductCategories.Remove(productCategory);
                            }
                            await _db.SaveChangesAsync();  
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            });
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
