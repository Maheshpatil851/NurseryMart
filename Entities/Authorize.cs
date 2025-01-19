using Microsoft.EntityFrameworkCore;
using NurseryMart.IRepository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NurseryMart.Entities
{
    public class Authorize : IEntity
    {
        [Key]  // Define primary key using EF Core's Key attribute
        public int Id { get; set; }  // Use int or another appropriate type for primary key
        [Column("mobile")]
        public string Mobile { get; set; }  // Field for Mobile
        [Column("username")]
        public string UserName { get; set; }  // Field for UserName
        [Column("email")]
        public string Email { get; set; }  // Field for Email
        [Column("passsword")]
        public string? Password { get; set; }  // Field for Password (ensure you hash it properly for security)
        [Column("address")]
        public string? Address { get; set; }  // Field for Address
        [Column("country")]
        public string? Country { get; set; }  // Field for Country
        [Column("state")]
        public string? State { get; set; }  // Field for State
        [Column("city")]
        public string? City { get; set; }  // Field for City
        [Column("pincode")]
        public string? Pincode { get; set; }  // Field for Pincode
                                              // Foreign Key to Trail (One-to-many relationship)
        public int? TrailId { get; set; }
        [JsonIgnore]// Nullable if you want to allow Authorize without a Trail reference
        public Trail Trail { get; set; }  // Navigation property to Trail
    }
}
