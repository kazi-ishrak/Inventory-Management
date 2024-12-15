using Inventory_Management.Data;
using Inventory_Management.Models;
using Inventory_Management.Services;
using static Inventory_Management.Models.DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Inventory_Management.Handler;


namespace Inventory_Management.Repositories
{
    public class ProductRepository : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>?> GetAll()
        {
            try
            {
                var query = await _context.Products.ToListAsync();
                /*var query = await _context.Products
                    .Select(p => new ProductCategoryDTO
                    {
                        ProductID = p.Id,
                        ProductName = p.Name,
                        SKU = p.Sku,
                        Stock = p.Stock,
                        Price = p.Price,
                        ProductCreated = p.Created_at,
                        ProductUpdated = p.Updated_at,
                        Categories = p.ProductCategories
                            .OrderBy(pc => pc.Category.Name)
                            .Select(pc => pc.Category.Name)
                            .Aggregate((c1, c2) => c1 + ", " + c2)
                    })
                    .ToListAsync();*/

                return query;
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return null;
            }
        }

        public async Task<Product> GetById(long Id)
        {
            try
            {
                var query = await _context.Products.Where(x=> x.Id==Id).FirstOrDefaultAsync();
                /*var query = await _context.Products
                    .Select(p => new ProductCategoryDTO
                    {
                        ProductID = p.Id,
                        ProductName = p.Name,
                        SKU = p.Sku,
                        Stock = p.Stock,
                        Price = p.Price,
                        ProductCreated = p.Created_at,
                        ProductUpdated = p.Updated_at,
                        Categories = p.ProductCategories
                            .OrderBy(pc => pc.Category.Name)
                            .Select(pc => pc.Category.Name)
                            .Aggregate((c1, c2) => c1 + ", " + c2)
                    })
                    .ToListAsync();*/
                return query;
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return null;
            }
        }


        public async Task Update(Product Input)
        {
            
        }
    }
}
