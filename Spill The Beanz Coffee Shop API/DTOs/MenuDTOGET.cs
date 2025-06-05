using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class MenuDTOGET //This is used in the Menu Items controller to display menu items for customers. 
    {
        public int ItemId { get; set; } 
        public string MenuCategory { get; set; }
        public string ItemName { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}
