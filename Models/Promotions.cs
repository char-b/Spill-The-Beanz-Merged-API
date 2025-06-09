using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class Promotions
    {
        [Key]
        [Column("promotion_id")]
        public int PromoID { get; set; }

        [Column("admin_id")]
        public int AdminID { get; set; }

        [Column("promo_name")]
        public string PromoName { get; set; }

        [Column("promo_description")]
        public string PromoDescription { get; set; }
        [Column("discount_type")]
        public string DiscountType { get; set; }

        [Column("discount_value")]
        public decimal DiscountValue { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("min_order_amount")]
        public decimal MinOrder {  get; set; } //?
                                               // public int applicableCategoryId { get; set; }

        [Column("applicable_item_id")]
        public int ApplicableItemId { get; set; } //why not use this instead of above?

        [Column("status")]
        public string PromoStatus { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public ICollection<CustomerPromotions> SentPromotions { get; set; }
        public ICollection<Admin> Admins { get; set; } //Many promotions created by many admins
        //public ICollection<PromotionMenu> PromotionMenus { get; set; }

    }
}
