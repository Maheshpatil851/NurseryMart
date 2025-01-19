using System.ComponentModel.DataAnnotations;

namespace NurseryMart.Entities
{
    public class Category
    {
        [Key]  // Define primary key using EF Core's Key attribute
        public int Id { get; set; }  // Use int or another appropriate type for primary key
    }
}
