using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NurseryMart.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }                      
        public int CustomerId { get; set; }                    
        public DateTime OrderDate { get; set; }                
        public decimal TotalAmount { get; set; }               
        public decimal DiscountAmount { get; set; }           
        public string OrderStatus { get; set; }                
        public string PaymentMethod { get; set; }             
        public string ShippingAddress { get; set; }            
        public DateTime? ShippedDate { get; set; }            
        public DateTime? DeliveredDate { get; set; }           
        public string TrackingNumber { get; set; }             
        public string CustomerNote { get; set; }               
        public bool IsPaid { get; set; }                      
        public bool IsRefunded { get; set; }                  
        public string RefundStatus { get; set; }               
        public decimal TaxAmount { get; set; }                
        public decimal ShippingCost { get; set; }              
        public string PromoCode { get; set; }                 
        public string Currency { get; set; }
        public int? TrailId { get; set; }
        [JsonIgnore]
        public Trail Trail { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Authorize Authorize { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetails> OrderDetails { get; set; }  // Collection of order details (many-to-many with Product)
    }
}
