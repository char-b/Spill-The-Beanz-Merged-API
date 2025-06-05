using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class CustomerPromotions //join-table?
    {
        [Key]
        public int Id { get; set; } //pk
        public int CustomerId { get; set; }
        public int PromotionId { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DateRedeemed { get; set; }

        //promoStatus recevies the values Available/Redeemed/Expired
        public string PromoStatus { get; set; }
        public virtual Customers Customer { get; set; } = null!;
        public virtual Promotions Promotion { get; set; } = null!;
    }
}
