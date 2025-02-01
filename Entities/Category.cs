using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NurseryMart.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? TrailId { get; set; }
        [JsonIgnore]
        public Trail Trail { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } 
        public int? ParentCategoryId { get; set; }  // Nullable for root categories
        public Category ParentCategory { get; set; }
        [JsonIgnore]
        public ICollection<Category> SubCategories { get; set; }  // A category can have many subcategories
    }
}
