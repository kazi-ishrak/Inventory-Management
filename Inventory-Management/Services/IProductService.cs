using Inventory_Management.Models;
using static Inventory_Management.Models.DatabaseModel;
using static System.Net.Mime.MediaTypeNames;

namespace Inventory_Management.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll();
        public Task<Product> GetById(long Id);
        public Task Update(Product Input);
    }
}
