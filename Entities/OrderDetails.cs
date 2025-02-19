using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NurseryMart.Entities
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set; }                 // Primary key
        public int? OrderId { get; set; }                        // Foreign key to Order
        public int ProductId { get; set; }                      // Foreign key to Product
        public int Quantity { get; set; }                       // Quantity of the product in the order
        public decimal Price { get; set; }                      // Price of the product at the time of the order
        public decimal Discount { get; set; }                   // Discount on the product (if any)
        public string Mobile { get; set; }
        public string AlternateMobile { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public int CustomerId { get; set; } 
        public decimal SubTotal { get; set; }                   // Subtotal for this product (Quantity * Price)
        public int? TrailId { get; set; }
        [JsonIgnore]
        public Trail Trail { get; set; }                        // Navigation property for Trail 
        // Navigation properties
        [JsonIgnore]
        public Order Order { get; set; }                        // Reference to the related order
        [JsonIgnore]
        public Product Product { get; set; }                    // Reference to the related product
    }
}
