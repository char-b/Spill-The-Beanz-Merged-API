using System.ComponentModel.DataAnnotations;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class UpdateOrderStatusDto
    {
        [Required]
        [MinLength(3)]
        public string Status { get; set; } = string.Empty;
    }
}
