using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class Admin
    {
        [Key]
        [Column("admin_id")]
        public int AdminId { get; set; }

        [Column("name")]
        public string adminName { get; set; } 

        [Column("email_address")]

        public string Email { get; set; }

        [Column("password_hash")]
        public string Password { get; set; }

        [Column("created_at")]
        public DateTime profileCreatedAt { get; set; } //when inserting it, it must just fetch the current date. when clicking, it invokes

        [Column("last_login")]
        public DateTime? lastLogin { get; set; } //change to login


        public ICollection<Promotions> Promotions { get; set; } = new List<Promotions>();

        public ICollection<Orders> Orders { get; set; } = new List<Orders>();








    }
}
