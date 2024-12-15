using Inventory_Management.Data;
using Inventory_Management.Services;
using Microsoft.EntityFrameworkCore;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Repositories
{
    public class UserRepository : IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task Create(User input)
        {
            _db.Users.Add(input);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> GetById(long id)
        {
            return await _db.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(long id)
        {
            User data = await GetById(id);
            if (data != null)
            {
                _db.Users.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(User input)
        {
            if (input != null)
            {
                _db.Users.Update(input);
                await _db.SaveChangesAsync();
            }
        }
    }
}
