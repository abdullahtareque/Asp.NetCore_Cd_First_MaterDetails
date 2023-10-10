using Microsoft.EntityFrameworkCore;
namespace Core_Master.Models
{
    public class myContext:DbContext
    {
        public myContext(DbContextOptions<myContext> options) : base(options)
        {

        }
        public DbSet<OrderMaster> OrderMasters { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.OrderMaster)
                .WithMany(om => om.OrderDetails)
                .HasForeignKey(od => od.OrderId);
        }

    }
}
