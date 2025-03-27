using Microsoft.EntityFrameworkCore;
using webapp.Models;

namespace webapp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {   }

        public DbSet<CustomerList> CustomerLists { get; set; }
        public DbSet<ItemList> ItemLists { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderMaster> OrderMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderMaster>()
                .HasOne<CustomerList>()
                .WithMany()
                .HasForeignKey(o => o.CustomerID)
                .HasPrincipalKey(c => c.CustomerID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne<OrderMaster>()
                .WithMany()
                .HasForeignKey(d => d.OrderMasterID)
                .HasPrincipalKey(m => m.OrderMasterID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne<ItemList>()
                .WithMany()
                .HasForeignKey(d => d.ItemID)
                .HasPrincipalKey(i => i.ItemID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
