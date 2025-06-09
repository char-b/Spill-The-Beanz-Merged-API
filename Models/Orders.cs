using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class Orders // This is keeping track of is the order amount, times and not the items ordered?
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("order_type")]
        public string OrderType { get; set; } = null!;

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("discount_amount")]
        public decimal DiscountAmount { get; set; }

        [Column("tax_amount")]
        public decimal TaxAmount { get; set; }

        [Column("final_amount")]
        public decimal FinalAmount { get; set; }

        [Column("order_status")]
        public string OrderStatus { get; set; }

        [Column("special_instructions")]
        public string? SpecialInstructions { get; set; }

        [Column("reservation_id")]

        public int? ReservationId { get; set; }

        //delivery address needed?

        public virtual Customers Customer { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        [ForeignKey("ReservationId")]
        public virtual TableReservations TableReservation { get; set; }
        public ICollection<Admin> Admins { get; set; } = new List<Admin>();


    }
}
