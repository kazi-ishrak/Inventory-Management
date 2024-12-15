using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Services
{
    public interface ICategoryService
    {
        Task Create(Category input);
        Task<List<Category>> GetAll();
        Task<Category> GetById(long id);
        Task Delete(long id);
        Task Update(Category input);
    }
}
