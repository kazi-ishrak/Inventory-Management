namespace Inventory_Management.Models
{
    public class ProductCategoryDTO
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public DateTime ProductCreated { get; set; }
        public DateTime ProductUpdated { get; set; }
        public string Categories { get; set; }
    }

}


