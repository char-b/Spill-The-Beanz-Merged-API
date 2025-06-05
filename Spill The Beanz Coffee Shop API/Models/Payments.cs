using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class Payments
    {
        [Key]
        [Column("payment_id")]
        public int payment_id { get; set; }
        [Column("order_id")]
        public int order_id { get; set; }
        [Column("payment_method")]
        public string payment_method { get; set; }
        [Column("payment_amount")]
        public decimal payment_amount { get; set; }
        [Column("payment_status")]
        public string payment_status { get; set; }
        [Column("transaction_reference")]
        public string transaction_reference { get; set; }
        [Column("payment_date")]
        public DateTime payment_date { get; set; }
        [ForeignKey("order_id")]
        public virtual Orders Orders { get; set; }
    }
}
