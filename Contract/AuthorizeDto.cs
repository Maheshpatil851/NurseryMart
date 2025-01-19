using System.ComponentModel.DataAnnotations;

namespace NurseryMart.Contract
{
    public class AuthorizeDto
    {
        public required string Mobile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public char Gender { get; set; }
    }
}
