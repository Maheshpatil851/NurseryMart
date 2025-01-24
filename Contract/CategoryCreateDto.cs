using NurseryMart.Entities;
using System.Text.Json.Serialization;

namespace NurseryMart.Contract
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? ParentCategoryId { get; set; }  
    }
}
