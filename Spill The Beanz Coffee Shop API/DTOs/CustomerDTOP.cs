using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class CustomerDTOP //Used in the customers controller when patching. The user should be able to change their details (inclduing password which is not contained in the CustomerDTOGet DTO for security.
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
    }
}
