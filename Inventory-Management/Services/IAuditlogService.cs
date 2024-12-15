using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Services
{
    public interface IAuditLogService
    {
        Task Create(AuditLog input);
        Task<List<AuditLog>> GetAll();
        Task<AuditLog> GetById(int id);
        Task Delete(int id);
        Task Update(AuditLog input);
    }
}