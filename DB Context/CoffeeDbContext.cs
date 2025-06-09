using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.DB_Context
{
    public class CoffeeDbContext : DbContext
    {


        public CoffeeDbContext(DbContextOptions<CoffeeDbContext> options) : base(options)
        {

        }

        public DbSet<Customers> Customers { get; set; } = null!;
        public DbSet<Orders> Orders { get; set; } = null;
        public DbSet<Tables> Tables { get; set; } = null!;
        public DbSet<TableReservations> TableReservations { get; set; } = null!;
        public DbSet<Menu> Menu { get; set; } = null!;
        public DbSet<OrderItems> OrderItems { get; set; } = null!;
        public DbSet<Payments> Payments { get; set; } = default!;
        public DbSet<Admin> Admin { get; set; } = null;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Item)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .HasConstraintName("FK_OrderItems_Menu");


            base.OnModelCreating(modelBuilder);
      

        }
    }


    
}
