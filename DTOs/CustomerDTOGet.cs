using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class CustomerDTOGet //This is to fetch customer details in general. Used in Customer Controller
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        //public List<CustomerDTOOrderGET> Orders { get; set; }
    }
}
