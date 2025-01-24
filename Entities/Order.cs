using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NurseryMart.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }                      // Primary key
        public int CustomerId { get; set; }                    // Foreign key to Customer
        public DateTime OrderDate { get; set; }                // Date the order was placed
        public decimal TotalAmount { get; set; }               // Total amount of the order
        public decimal DiscountAmount { get; set; }            // Discount applied to the order
        public string OrderStatus { get; set; }                // Status of the order (Pending, Shipped, etc.)
        public string PaymentMethod { get; set; }              // Payment method used by the customer
        public string ShippingAddress { get; set; }            // Shipping address for the order
        public DateTime? ShippedDate { get; set; }             // Date the order was shipped
        public DateTime? DeliveredDate { get; set; }           // Date the order was delivered
        public string TrackingNumber { get; set; }             // Tracking number for shipment (optional)
        public string CustomerNote { get; set; }               // Special note from the customer
        public bool IsPaid { get; set; }                       // Whether the order is paid
        public bool IsRefunded { get; set; }                   // Whether the order is refunded
        public string RefundStatus { get; set; }               // Refund status (if applicable)
        public decimal TaxAmount { get; set; }                 // Tax amount for the order
        public decimal ShippingCost { get; set; }              // Shipping cost for the order
        public string PromoCode { get; set; }                  // Promo code applied (if applicable)
        public string Currency { get; set; }                   // Currency used (e.g., USD)

        public int? TrailId { get; set; }
        [JsonIgnore]
        public Trail Trail { get; set; }// Whether the order is marked as deleted


        // Navigation properties
        [JsonIgnore]
        public Authorize Authorize { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetails> OrderDetails { get; set; }  // Collection of order details (many-to-many with Product)
    }
}
