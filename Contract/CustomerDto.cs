using System.ComponentModel.DataAnnotations;

namespace NurseryMart.Contract
{
    public class CustomerDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public char Gender { get; set; }
    }
}
