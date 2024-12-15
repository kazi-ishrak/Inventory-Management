using Inventory_Management.Data;
using Inventory_Management.Services;
using Microsoft.EntityFrameworkCore;
using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Repositories
{
    public class AuditLogRepository : IAuditLogService
    {
        private readonly ApplicationDbContext _db;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task Create(AuditLog input)
        {
            _db.AuditLogs.Add(input);
            await _db.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetAll()
        {
            return await _db.AuditLogs.ToListAsync();
        }

        public async Task<AuditLog> GetById(int id)
        {
            return await _db.AuditLogs
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(int id)
        {
            AuditLog data = await GetById(id);
            if (data != null)
            {
                _db.AuditLogs.Remove(data);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(AuditLog input)
        {
            if (input != null)
            {
                _db.AuditLogs.Update(input);
                await _db.SaveChangesAsync();
            }
        }
    }
}
