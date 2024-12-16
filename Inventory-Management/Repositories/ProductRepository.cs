using Inventory_Management.Data;
using Inventory_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Repositories
{
    public class ProductRepository : IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task Create(Product input)
        {
            _db.Products.Add(input);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAll()
        {
            // Start with IQueryable for flexibility and deferred execution
            var productsQuery = _db.Products
                                   .AsQueryable()  // Defers execution until the query is actually evaluated
                                   .Include(p => p.ProductCategories) // Eager load the related ProductCategories
                                   .ThenInclude(pc => pc.Category); // Eager load the related Category for each ProductCategory

            return await productsQuery.ToListAsync();
        }


        public async Task<Product> GetById(long id)
        {
            return await _db.Products
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
            if (input != null)
            {
                _db.Products.Update(input);
                await _db.SaveChangesAsync();
            }
        }
    }
}
