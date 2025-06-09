using System.ComponentModel.DataAnnotations.Schema;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class CustomerDTORes //This is used in TableReservations Controller. 
    {
        public int ReservationId { get; set; } //Frontend expects reservation ID first, so it's included here

        //Customer information 
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        //One customer can have many table reservations. This links to the DTO so we only receive certain information. 
        public List<TableResDTO> TableReservations { get; set; }
    }
}
