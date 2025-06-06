using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class TableResDTO //This is used in the TableReservationsController.
    {      
        public int TableId { get; set; }
        public DateOnly ReservationDate { get; set; }
          public TimeOnly StartTime { get; set; }
         public TimeOnly EndTime { get; set; }
        public int PartySize { get; set; }
        public string? SpecialRequests { get; set; }
        public string ReservationStatus { get; set; }

    }
}
