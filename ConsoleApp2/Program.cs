using ConsoleApp2;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase("CustomerOrders")
    .Options;

await using var context = new AppDbContext(options);
await context.Database.EnsureCreatedAsync();

if (!await context.Customers.AnyAsync())
{
    context.Customers.Add(new Customer
    {
        Name = "Contoso",
        Orders =
        [
            new Order
            {
                Status = "Pending",
                Items =
                [
                    new OrderItem { ProductName = "Keyboard", Quantity = 1 },
                    new OrderItem { ProductName = "Mouse", Quantity = 2 }
                ]
            }
        ]
    });

    await context.SaveChangesAsync();
}

var repository = new AppRepository(context);
var customer = await repository.GetCustomerWithOrdersAsync(1);

if (customer is null)
{
    Console.WriteLine("Customer not found.");
    return;
}

customer.Name = "Contoso Ltd.";

foreach (var order in customer.Orders)
{
    order.Status = "Processing";

    foreach (var item in order.Items)
    {
        item.Quantity += 1;
    }
}

await repository.SaveChangesAsync();

var updatedCustomer = await repository.GetCustomerWithOrdersAsync(customer.Id);
Console.WriteLine($"Customer: {updatedCustomer!.Name}");

foreach (var order in updatedCustomer.Orders)
{
    Console.WriteLine($"Order {order.Id}: {order.Status}");

    foreach (var item in order.Items)
    {
        Console.WriteLine($"  {item.ProductName}: {item.Quantity}");
    }
}
