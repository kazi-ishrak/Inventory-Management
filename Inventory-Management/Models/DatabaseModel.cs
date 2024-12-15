using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Inventory_Management.Models
{
    public class DatabaseModel
    {
        [Table("products")]
        public class Product
        {
            [Key]
            [Column("id")]
            public long Id { get; set; } // Primary Key

            [Required]
            [MaxLength(100)]
            [Column("name")]
            public string Name { get; set; } // Product Name

            [Required]
            [MaxLength(50)]
            [Column("sku")]
            public string Sku { get; set; } // Stock Keeping Unit (SKU)

            [Required]
            [Column("stock")]
            public int Stock { get; set; }

            [Required]
            [Column("price", TypeName = "decimal(18,2)")]
            public decimal Price { get; set; } // Price with two decimal places

            [Required]
            [Column("created_at")]
            public DateTime Created_at { get; set; } // Timestamp for record creation

            [Required]
            [Column("updated_at")]
            public DateTime Updated_at { get; set; } // Timestamp for the last update


            public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        }

        [Table("categories")]
        public class Category
        {
            [Key]
            [Column("id")]
            public long Id { get; set; } // Primary Key

            [Required]
            [MaxLength(100)]
            [Column("name")]
            public string Name { get; set; } // Product Name
            [Required]
            [Column("created_at")]
            public DateTime Created_at { get; set; } // Timestamp for record creation

            [Required]
            [Column("updated_at")]
            public DateTime Updated_at { get; set; } // Timestamp for the last update
        }

        [Table("product_categories")]
        public class ProductCategory
        {
            [Key]
            [Column("id")]
            public long Id { get; set; } // Primary Key

            [Required]

            [Column("product_id")]
            public long ProductId { get; set; } // Product Name


            [Required]
            [Column("category_id")]
            public long CategoryId { get; set; } // Timestamp for the last update
        }

        [Table("users")]
        public class User
        {
            [Key]
            [Column("id")]
            public long Id { get; set; } // Primary Key

            [Required]
            [MaxLength(100)]
            [Column("name")]
            public string Name { get; set; } // User Name

            [Required]
            [Column("created_at")]
            public DateTime CreatedAt { get; set; } // Timestamp for record creation

            [Required]
            [MaxLength(50)]
            [Column("role")]
            public string Role { get; set; } // User Role
        }

        [Table("audit_logs")]
        public class AuditLog
        {
            [Key]
            [Column("audit_id")]
            public long Id { get; set; } // Primary Key

            [Required]
            [Column("product_id")]
            public long ProductId { get; set; }

            [Required]
            [Column("timestamp")]
            public DateTime Timestamp { get; set; }

            [Required]
            [MaxLength(50)]
            [Column("change_type")]
            public string ChangeType { get; set; } // Type of change

            [Required]
            [Column("quantity")]
            public int Quantity { get; set; }

            [Required]
            [Column("user_id")]
            public long UserId { get; set; }

            [Required]
            [Column("created_at")]
            public DateTime CreatedAt { get; set; } // Timestamp for record creation
        }
    }
}
