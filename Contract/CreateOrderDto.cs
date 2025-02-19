namespace NurseryMart.Contract
{
    public class CreateOrderDto
    {
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public string AlterNateMobileNumber { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
    }
}
