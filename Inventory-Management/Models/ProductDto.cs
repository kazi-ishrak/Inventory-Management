namespace Inventory_Management.Models
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }
}
