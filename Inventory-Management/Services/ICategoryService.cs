using static Inventory_Management.Models.DatabaseModel;


namespace Inventory_Management.Services
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetAll();

    }
}
