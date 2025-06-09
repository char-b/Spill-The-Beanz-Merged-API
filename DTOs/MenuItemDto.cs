using System.ComponentModel.DataAnnotations;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class MenuItemDto
    {
        [Required]
        public string ItemName { get; set; }

        [Required]
        public string MenuCategory { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }

        public bool? IsAvailable { get; set; }
        public bool? IsFeatured { get; set; }

        [Url]
        public string? ImageUrl { get; set; }
    }
}

