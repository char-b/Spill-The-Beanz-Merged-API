using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class Menu
    {
        [Key]
        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("is_available")]
        public bool? IsAvailable { get; set; }

        [Column("is_featured")]
        public bool? IsFeatured { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [InverseProperty("Item")]
        public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

        //public ICollection<PromotionMenu> PromotionMenus { get; set; }

        public virtual ICollection<Promotions> promotions { get; set; } = new List<Promotions>();
    }
}
