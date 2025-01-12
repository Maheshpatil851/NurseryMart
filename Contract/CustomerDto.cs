using System.ComponentModel.DataAnnotations;

namespace NurseryMart.Contract
{
    public class CustomerDTO
    {
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
