using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class TableResDTOPatch //This is used in the TableReservationsController.
    {      
        public int TableId { get; set; }
        public string ReservationStatus { get; set; }

    }
}
