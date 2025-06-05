using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class OrderItems
    {
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }  // <— This is your FK column

        // This navigation uses ItemId (mapped to item_id), 
        // and its inverse on Menu is the OrderItems collection
        [ForeignKey("ItemId")]
        [InverseProperty("OrderItems")]    // <— Add this line
        public virtual Menu Item { get; set; } = null!;

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("special_requests")]
        public string? SpecialRequests { get; set; }

        [Column("item_status")]
        public string ItemStatus { get; set; }

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("OrderId")]
        public virtual Orders Order { get; set; } = null!;
    }
}
