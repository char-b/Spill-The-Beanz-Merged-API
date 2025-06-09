using System.ComponentModel.DataAnnotations.Schema;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class CustomerDTOOrderGET //This is to fetch customer details for orders. Used in Customer Controller
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
