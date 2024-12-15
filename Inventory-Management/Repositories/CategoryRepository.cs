using Inventory_Management.Services;
using Inventory_Management.Data;
using static Inventory_Management.Models.DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Inventory_Management.Handler;
namespace Inventory_Management.Repositories
{
    public class CategoryRepository : ICategoryService
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository (ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        public async Task<List<Category>?> GetAll()
        {
            try
            {
                return await _db.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                LogHandler.WriteErrorLog(ex);
                return null;
            }
        }
    }
}
