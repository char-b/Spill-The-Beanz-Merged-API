using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class Tables
    {
        [Key]
        [Column("table_id")]
        public int TableId { get; set; }

        [Column("table_number")]
        public string TableNumber { get; set; } //this is unique

        [Column("is_available")]
        public bool TableAvailability { get; set; }

        [Column("capacity")]
        public int TableCapacity { get; set; }

        [Column("location_description")]
        public string TableLocation { get; set; }

        //One table has a 'collection' of table reservations
        public ICollection<TableReservations> tableReservations { get; set; }

    }
}
