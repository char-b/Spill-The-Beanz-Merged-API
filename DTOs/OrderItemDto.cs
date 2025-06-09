using System.ComponentModel.DataAnnotations;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class OrderItemDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
