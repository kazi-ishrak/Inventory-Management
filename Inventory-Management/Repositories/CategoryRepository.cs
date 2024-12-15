using Inventory_Management.Data;
using Inventory_Management.Services;
using Microsoft.EntityFrameworkCore;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Repositories
{
    public class CategoryRepository : ICategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task Create(Category input)
        {
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

        public async Task Delete(long id)
        {
            Category data = await GetById(id);
            if (data != null)
            {
                _db.Categories.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(Category input)
        {
            if (input != null)
            {
                _db.Categories.Update(input);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Category> GetById(long Id)
        {
            try
            {
                var Query = await _db.Categories.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return Query;
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return null;
            }
        }
    }
}
