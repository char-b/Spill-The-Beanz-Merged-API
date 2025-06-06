namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class RegisterCustomerDto
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Password { get; set; } // Plaintext sent from frontend
    }
}
