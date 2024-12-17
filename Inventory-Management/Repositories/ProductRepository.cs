using Inventory_Management.Data;
using Inventory_Management.Models;
using Inventory_Management.Services;
using Microsoft.EntityFrameworkCore;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Repositories
{
    public class ProductRepository : IProductService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuditLogService _auditLogService;

        public ProductRepository(ApplicationDbContext context, IAuditLogService auditLogService)
        {
            _db = context;
            _auditLogService = auditLogService;
        }

        public async Task Create(Product input)
        {
            if(input == null || input.Stock < 0)
            {
                return;
            }
            _db.Products.Add(input);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetAll()
        {
            // Start with IQueryable for flexibility and deferred execution
            var productsQuery = _db.Products
                                   .AsQueryable()
                                   .Include(p => p.ProductCategories)  // Eager load the related ProductCategories
                                   .ThenInclude(pc => pc.Category);    // Eager load the related Category for each ProductCategory

            // Use LINQ to project the data into a list of DTOs
            var productDtos = await productsQuery
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Sku = p.Sku,
                    Stock = p.Stock,
                    Price = p.Price,
                    Created_at = p.Created_at,
                    Updated_at = p.Updated_at,
                    Categories = p.ProductCategories.Select(pc => new CategoryDto
                    {
                        Id = pc.Category.Id,
                        Name = pc.Category.Name
                    }).ToList()
                })
                .ToListAsync();

            return productDtos;
        }


        public async Task<Product> GetById(long id)
        {
            return await _db.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Product?> GetByIdV2(long id)
        {
            return await _db.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(long id)
        {
            Product data = await GetById(id);
            if (data != null)
            {
                _db.Products.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(Product input)
        {
            if (input != null && input.Stock > 0)
            {
                var oldProduct = await GetByIdV2(input.Id);
                if (oldProduct != null)
                {
                    if (oldProduct.Stock != input.Stock)
                    {
                        await _auditLogService.Create(new()
                        {
                            ProductId = input.Id,
                            Timestamp = DateTime.Now,
                            ChangeType =  $"{input.Name}'s stock is changed to: {input.Stock}",
                            Quantity = input.Stock,
                            UserId = 0,
                            CreatedAt = DateTime.Now
                        });
                    }

                    _db.Products.Update(input);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
