using Microsoft.EntityFrameworkCore;
using HHPW_Serverside.Models;
using HHPW_Serverside.DTOs;

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
    public DbSet<CreateOrderPayload> CreateOrderPayload { get; set; }

    public hhpwDbContext(DbContextOptions<hhpwDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity("ItemOrder").Property<int>("ItemOrderId").HasColumnType("int").ValueGeneratedOnAdd();
        modelBuilder.Entity("ItemOrder").HasKey("ItemOrderId");
        // modelBuilder.Entity("ItemOrder").HasOne("OrdersId").WithMany();
        // modelBuilder.Entity("ItemOrder").HasOne("itemsId").WithMany();

    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<PaymentType>().HasData(new PaymentType[]
    //    {
    //        new PaymentType {Id = 1, paymentTypeDesc = "payment type 1"},
    //        new PaymentType {Id = 2, paymentTypeDesc = "payment type 2"}
    //    });

    //    modelBuilder.Entity<ItemType>().HasData(new ItemType[]
    //    {
    //        new ItemType {Id = 1, Name = "appetizer"},
    //        new ItemType {Id = 2, Name = "beverage"},
    //        new ItemType {Id = 3, Name = "entree"},
    //        new ItemType {Id = 4, Name = "dessert"}
    //    });

    //    modelBuilder.Entity<OrderType>().HasData(new OrderType[]
    //    {
    //        new OrderType {Id = 1, orderTypeName = "Order type 1"},
    //        new OrderType {Id = 2, orderTypeName = "Order type 2"}
    //    });

    //    modelBuilder.Entity<Review>().HasData(new Review[]
    //    {
    //        new Review {Id = 1, content = "Order 2 review", orderId = 2},
    //        new Review {Id = 2, content = "Order 1 review", orderId = 1},
    //        new Review {Id = 3, content = "Order 1 review 2", orderId = 1},
    //    });

    //    modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus[]
    //    {
    //        new OrderStatus {Id = 1, statusName = "Open"},
    //        new OrderStatus {Id = 2, statusName = "In Process"},
    //        new OrderStatus {Id = 3, statusName = "Closed"}
    //    });

    //    modelBuilder.Entity<Item>().HasData(new Item[]
    //    {
    //        new Item {Id = 1, itemTypeId = 4, itemName = "Pumpkin Pie", price = 8.00M},
    //        new Item {Id = 2, itemTypeId = 3, itemName = "Ahi Tuna", price = 30.00M},
    //        new Item {Id = 3, itemTypeId = 2, itemName = "Fountain Drink", price = 4.50M},
    //        new Item {Id = 4, itemTypeId = 1, itemName = "Nachos", price = 14.00M},

    //    });

    //    modelBuilder.Entity<User>().HasData(new User[]
    //    {
    //        new User {Id = 1, name = "test user 1", uid = "uid 1", isEmployee = true},
    //        new User {Id = 2, name = "Me Auth", uid = "azXwPE8IQRRHUi4pBGB4kGLeCL93", isEmployee = true}
    //    });
    //}
}