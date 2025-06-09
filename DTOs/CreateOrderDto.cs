using System.ComponentModel.DataAnnotations;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<OrderItemDto> Items { get; set; } = new();
    }

}
