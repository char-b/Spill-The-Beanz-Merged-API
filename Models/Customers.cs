using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Spill_The_Beanz_Coffee_Shop_API.Models
{
    public class Customers
    {
        [Key]
        [Column("customer_id")]   //Need to explicitly tell EF that this column maps to the customer_id column in the database
        public int CustomerID { get; set; }

        [Column("customer_name")]
        public string CustomerName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("address")]
        public string? Address { get; set; } //should they input this when registering. not required

        [Column("loyalty_points")]
        public int? LoyaltyPoints { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } //when inserting it, it must just fetch the current date. when clicking, it invokes
        
        [Column("last_visited")]
        public DateTime? LastVisited { get; set; } //when they log out. sessions

        [Column("is_active")]
        public bool? IsActive { get; set; }
        //adding these will create additional links
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public ICollection<TableReservations> TableReservations { get; set; } = new List<TableReservations>(); //ok if same name accross classes? not neccessary to declare tb reservation in customers right?
        public ICollection<CustomerPromotions> CustomerPromotions { get; set; } = new List<CustomerPromotions>();
    }
}
