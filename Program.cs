using HHPW_Serverside.Models;
using HHPW_Serverside.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:7040")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<hhpwDbContext>(builder.Configuration["hhpwDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

//Add for Cors 
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// USER ENDPOINTS

// check for authenticated/registered user
app.MapGet("/checkuser/{uid}", (hhpwDbContext db, string uid) =>
{
    var authUser = db.users?.Where(u => u.uid == uid).ToList();
    if (uid == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(authUser);
    }
});

//get all users
app.MapGet("/users", (hhpwDbContext db) =>
{
    return db.users.Include(x => x.orders)
                   .ThenInclude(o => o.items)
                   .ToList();
});

//get single user by uid
app.MapGet("/userByUid/{uid}", (hhpwDbContext db, string uid) =>
{
    return db.users.Where(u => u.uid == uid)
                   .Include(x => x.orders)
                   .ThenInclude(o => o.items)
                   .ToList();
});

//get single user by db id
app.MapGet("/users/{id}", (hhpwDbContext db, int id) =>
{
    return db.users.Where(u => u.Id == id)
                   .Include(x => x.orders)
                   .ThenInclude(o => o.items)
                   .ToList();
});

//create new user
app.MapPost("/users", (hhpwDbContext db, User userPayload) =>
{
    User NewUser = new User()
    {
        name = userPayload.name,
        isEmployee = userPayload.isEmployee,
        uid = userPayload.uid,
    };
    db.users.Add(NewUser);
    db.SaveChanges();
    return Results.Created($"/api/users/{NewUser.Id}", NewUser);
});

// delete user by id
app.MapDelete("/api/users/{id}", (hhpwDbContext db, int id) =>
{
    User userToDelete = db.users.SingleOrDefault(u => u.Id == id);
    if (userToDelete == null)
    {
        return Results.NotFound();
    }
    db.users.Remove(userToDelete);
    db.SaveChanges();
    return Results.Ok(db.users);
});

// update user by id
app.MapPut("/api/updateuser/{id}", (hhpwDbContext db, int id, User user) =>
{
    User userToUpdate = db.users.SingleOrDefault(u => u.Id == id);
    if (userToUpdate == null)
    {
        return Results.NotFound();
    }
    userToUpdate.name = user.name;
    userToUpdate.isEmployee = user.isEmployee;
    userToUpdate.uid = user.uid;
    db.SaveChanges();
    return Results.Ok(userToUpdate);
});

// View users orders
app.MapGet("/userorders/{id}/", (hhpwDbContext db, int id) =>
{

    return db.users.Where(u => u.Id == id)
                   .Include(u => u.orders)
                   .ThenInclude(o => o.items)
                   .ThenInclude(i => i.itemType)
                   .ToList();
});

// ITEM ENDPOINTS

// View all items
app.MapGet("/items", (hhpwDbContext db) =>
{
    return db.items.Include(i => i.Orders)
                   .Include(i => i.itemType)
                   .ToList();
});

// View single item by id
app.MapGet("/items/{id}", (hhpwDbContext db, int id) =>
{
    return db.items.Where(i => i.Id == id)
                   .Include(i => i.Orders)
                   .Include(i => i.itemType)
                   .ToList();
});

// Create Item
app.MapPost("/item", (hhpwDbContext db, Item itemPayload) =>
{
    Item NewItem = new Item()
    {
        itemTypeId = itemPayload.itemTypeId,
        itemName = itemPayload.itemName,
        price = itemPayload.price,
    };
    db.items.Add(NewItem);
    db.SaveChanges();
    return Results.Created($"/api/item/{NewItem.Id}", NewItem);
});

// update item by id
app.MapPut("/api/updateitem/{id}", (hhpwDbContext db, int id, Item itemPayload) =>
{
    Item itemToUpdate = db.items.SingleOrDefault(i => i.Id == id);
    if (itemToUpdate == null)
    {
        return Results.NotFound();
    }
    itemToUpdate.itemName = itemPayload.itemName;
    itemToUpdate.itemTypeId = itemPayload.itemTypeId;
    itemToUpdate.price = itemPayload.price;
    db.SaveChanges();
    return Results.Ok(itemToUpdate);
});

// delete item by id
app.MapDelete("/api/item/{id}", (hhpwDbContext db, int id) =>
{
    Item itemToDelete = db.items.SingleOrDefault(i => i.Id == id);
    if (itemToDelete == null)
    {
        return Results.NotFound();
    }
    db.items.Remove(itemToDelete);
    db.SaveChanges();
    return Results.Ok(db.items);
});

// ITEM TYPE ENDPOINTS

// create item type
app.MapPost("/itemType", (hhpwDbContext db, ItemType itemTypePayload) =>
{
    ItemType NewItemType = new ItemType()
    {
        Name = itemTypePayload.Name,
    };
    db.itemTypes.Add(NewItemType);
    db.SaveChanges();
    return Results.Created($"/api/itemType/{NewItemType.Id}", NewItemType);
});

//view all item types
app.MapGet("/itemTypes", (hhpwDbContext db) =>
{
    return db.itemTypes.ToList();
});

// View single item type by id
app.MapGet("/itemTypes/{id}", (hhpwDbContext db, int id) =>
{
    return db.itemTypes.FirstOrDefault(i => i.Id == id);
});

// delete item type by id
app.MapDelete("/api/itemTypes/{id}", (hhpwDbContext db, int id) =>
{
    ItemType itemTypeToDelete = db.itemTypes.SingleOrDefault(i => i.Id == id);
    if (itemTypeToDelete == null)
    {
        return Results.NotFound();
    }
    db.itemTypes.Remove(itemTypeToDelete);
    db.SaveChanges();
    return Results.Ok(db.itemTypes);
});

// PAYMENT TYPE ENDPOINTS

// create payment type
app.MapPost("/paymentType", (hhpwDbContext db, PaymentType paymentTypePayload) =>
{
    PaymentType NewPaymentType = new PaymentType()
    {
        paymentTypeDesc = paymentTypePayload.paymentTypeDesc,
    };
    db.paymentTypes.Add(NewPaymentType);
    db.SaveChanges();
    return Results.Created($"/api/paymentType/{NewPaymentType.Id}", NewPaymentType);
});

//view all payment types
app.MapGet("/paymentTypes", (hhpwDbContext db) =>
{
    return db.paymentTypes.ToList();
});

// View single payment type by id
app.MapGet("/paymentTypes/{id}", (hhpwDbContext db, int id) =>
{
    return db.paymentTypes.FirstOrDefault(i => i.Id == id);
});

// delete payment type by id
app.MapDelete("/api/paymentTypes/{id}", (hhpwDbContext db, int id) =>
{
    PaymentType paymentTypeToDelete = db.paymentTypes.SingleOrDefault(i => i.Id == id);
    if (paymentTypeToDelete == null)
    {
        return Results.NotFound();
    }
    db.paymentTypes.Remove(paymentTypeToDelete);
    db.SaveChanges();
    return Results.Ok(db.paymentTypes);
});

// ORDER STATUS ENDPOINTS

// create order status
app.MapPost("/orderStatus", (hhpwDbContext db, OrderStatus orderStatusPayload) =>
{
    OrderStatus NewOrderStatus = new OrderStatus()
    {
        statusName = orderStatusPayload.statusName,
    };
    db.orderStatuses.Add(NewOrderStatus);
    db.SaveChanges();
    return Results.Created($"/api/orderStatus/{NewOrderStatus.Id}", NewOrderStatus);
});

//view all order statuses
app.MapGet("/orderStatuses", (hhpwDbContext db) =>
{
    return db.orderStatuses.ToList();
});

// View single order status by id
app.MapGet("/orderStatuses/{id}", (hhpwDbContext db, int id) =>
{
    return db.orderStatuses.FirstOrDefault(i => i.Id == id);
});

// delete order status by id
app.MapDelete("/api/orderStatuses/{id}", (hhpwDbContext db, int id) =>
{
    OrderStatus statusToDelete = db.orderStatuses.SingleOrDefault(i => i.Id == id);
    if (statusToDelete == null)
    {
        return Results.NotFound();
    }
    db.orderStatuses.Remove(statusToDelete);
    db.SaveChanges();
    return Results.Ok(db.orderStatuses);
});

// ORDER TYPE ENDPOINTS

// create order type
app.MapPost("/orderType", (hhpwDbContext db, OrderType orderTypePayload) =>
{
    OrderType NewOrderType = new OrderType()
    {
        orderTypeName = orderTypePayload.orderTypeName,
    };
    db.orderTypes.Add(NewOrderType);
    db.SaveChanges();
    return Results.Created($"/api/orderType/{NewOrderType.Id}", NewOrderType);
});

//view all order types
app.MapGet("/orderTypes", (hhpwDbContext db) =>
{
    return db.orderTypes.ToList();
});

// View single order types by id
app.MapGet("/orderTypes/{id}", (hhpwDbContext db, int id) =>
{
    return db.orderTypes.FirstOrDefault(i => i.Id == id);
});

// delete order types by id
app.MapDelete("/api/orderTypes/{id}", (hhpwDbContext db, int id) =>
{
    OrderType orderTypeToDelete = db.orderTypes.SingleOrDefault(i => i.Id == id);
    if (orderTypeToDelete == null)
    {
        return Results.NotFound();
    }
    db.orderTypes.Remove(orderTypeToDelete);
    db.SaveChanges();
    return Results.Ok(db.orderTypes);
});

// ORDER ENDPOINTS

// create an order
app.MapPost("/orders", (hhpwDbContext db, CreateOrderPayload orderPayload) =>
{
    //if (orderPayload == null || orderPayload.itemIds == null || !orderPayload.itemIds.Any())
    //{
    //    return Results.BadRequest("Invalid request. You must provide at least one item ID.");
    //}

    Order NewOrder = new Order()
    {
        orderStatusId = orderPayload.orderStatusId,
        orderTypeId = orderPayload.orderTypeId,
        paymentTypeId = orderPayload.paymentTypeId,
        userId = orderPayload.userId,
        items = new List<Item>()
    };

    // Find and add items to the order based on item IDs
    foreach (var itemId in orderPayload.itemIds)
    {
        var itemToAdd = db.items.SingleOrDefault(i => i.Id == itemId);
        if (itemToAdd != null)
        {
            NewOrder.items.Add(itemToAdd);
        }
        else
        {
            return Results.NotFound($"Item with ID {itemId} not found.");
        }
    }

    db.orders.Add(NewOrder);
    db.SaveChanges();
    return Results.Created($"/api/orders/{NewOrder.Id}", NewOrder);
});

//view all orders
app.MapGet("/orders", (hhpwDbContext db) =>
{
    return db.orders.Include(o => o.status)
                    .Include(o => o.orderType)
                    .Include(o => o.paymentType)
                    .Include(o => o.user)
                    .Include(o => o.reviews)
                    .Include(o => o.items)
                    .ThenInclude(i => i.itemType)
                    .ToList();
});

// View single order by id
app.MapGet("/order/{id}", (hhpwDbContext db, int id) =>
{    
    return db.orders.Where(o => o.Id == id)
                    .Include(o => o.status)
                    .Include(o => o.orderType)
                    .Include(o => o.paymentType)
                    .Include(o => o.user)
                    .Include(o => o.reviews)
                    .Include(o => o.items)
                    .ThenInclude(i => i.itemType);
});

// delete order by id
app.MapDelete("/api/order/{id}", (hhpwDbContext db, int id) =>
{
    Order orderToDelete = db.orders.Include(o => o.items)
                                   .SingleOrDefault(o => o.Id == id); 
    if (orderToDelete == null)
    {
        return Results.NotFound();
    }

    decimal orderTotal = orderToDelete.items?.Sum(i => i.price) ?? 0;

    db.orders.Remove(orderToDelete);
    db.SaveChanges();
    return Results.Ok(db.orders);
});

// add item to order
app.MapPost("api/order/{orderId}/items/{itemId}", (hhpwDbContext db, int orderId, int itemId) =>
{
    var order = db.orders.Include(o => o.items)
                         .FirstOrDefault(o => o.Id == orderId);
    if (order == null)
    {
        return Results.NotFound("Order not found");
    }

    var itemToAdd = db.items?.Find(itemId);


    if (itemToAdd == null)
    {
        return Results.NotFound("Item not found");
    }

    order?.items?.Add(itemToAdd);
    db.SaveChanges();
    return Results.Ok(order);
});

// remove item from order
app.MapDelete("api/orders/{orderId}/items/{itemId}", (hhpwDbContext db, int orderId, int itemId) =>
{
    var order = db.orders
       .Include(o => o.items)
       .FirstOrDefault(o => o.Id == orderId);

    if (order == null)
    {
        return Results.NotFound("Order not found");
    }

    var itemToRemove = db.items.Find(itemId);

    if (itemToRemove == null)
    {
        return Results.NotFound("Item not found");
    }

    order.items.Remove(itemToRemove);
    db.SaveChanges();
    return Results.Ok(order);
});

// update order status by id
app.MapPut("/order/{id}", (hhpwDbContext db, int id, Order orderPayload) =>
{
    Order orderToUpdate = db.orders.SingleOrDefault(i => i.Id == id);
    if (orderToUpdate == null)
    {
        return Results.NotFound();
    }

    orderToUpdate.orderStatusId = orderPayload.orderStatusId;
    orderToUpdate.orderTypeId = orderPayload.orderTypeId;
    orderToUpdate.paymentTypeId = orderPayload.paymentTypeId;

    db.orders.Add(orderToUpdate);
    db.SaveChanges();
    return Results.Ok(orderToUpdate);
});



app.Run();

