namespace NurseryMart.Contract
{
    public class AccountResponseDto
    {
        public int Id { set; get; }
        public string? Email { get; set; }
        public required string Mobile { set; get; }
        public string? FirstName { set; get; }
        public string? LastName { set; get; }
        public char Gender { set; get; }
        public string? DpUrl { set; get; }
        public string? Country { set; get; }
        public string? State { set; get; }
        public string? City { set; get; }
        public string? PinCode { set; get; }
        public bool IsActive { set; get; }
    }
}
