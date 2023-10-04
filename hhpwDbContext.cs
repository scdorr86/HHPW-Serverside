using Microsoft.EntityFrameworkCore;
using HHPW_Serverside.Models;

public class hhpwDbContext : DbContext
{
    public DbSet<Order> orders { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<PaymentType> paymentTypes { get; set; }
    public DbSet<Item> items { get; set; }
    public DbSet<Review> reviews { get; set; }
    public DbSet<ItemType> itemTypes { get; set; }
    public DbSet<OrderType> orderTypes { get; set; }
    public DbSet<OrderStatus> orderStatuses { get; set; }

    public hhpwDbContext(DbContextOptions<hhpwDbContext> context) : base(context)
    {

    }
}