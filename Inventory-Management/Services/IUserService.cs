using static Inventory_Management.Models.DatabaseModel;

namespace Inventory_Management.Services
{
    public interface IUserService
    {
        Task Create(User input);
        Task<List<User>> GetAll();
        Task<User> GetById(long id);
        Task Delete(long id);
        Task Update(User input);
    }
}
