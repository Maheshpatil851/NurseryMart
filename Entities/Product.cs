using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NurseryMart.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }                  // Primary key
        public string Name { get; set; }                     // Product name
        public string Description { get; set; }              // Product description
        public decimal Price { get; set; }                   // Price of the product
        public decimal? DiscountPrice { get; set; }          // Discounted price
        public int CategoryId { get; set; }                 // Category the product belongs to
        public string SKU { get; set; }                      // Unique SKU for the product
        public int StockQuantity { get; set; }               // Stock available
        public string ImageUrl { get; set; }                 // Image URL for the product
        public bool IsActive { get; set; }                   // Is the product available for sale
        public DateTime CreatedAt { get; set; }              // Date the product was created
        public DateTime? ModifiedAt { get; set; }            // Date the product was last modified
        public DateTime? TrialEndDate { get; set; }          // Trial period end date (if applicable)
        public bool IsOnSale { get; set; }                   // Whether the product is on sale
        public decimal Rating { get; set; }                  // Average product rating
        public int ReviewsCount { get; set; }                // Number of reviews
        public string Brand { get; set; }                    // Brand of the product
        public bool IsFeatured { get; set; }                 // Is the product featured
                                                             // Foreign key to Trail
        public int? TrailId { get; set; }
        [JsonIgnore]
        public Trail Trail { get; set; }   // Navigation property for Trail
    }
}
