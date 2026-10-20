using Microsoft.EntityFrameworkCore;

namespace ConsoleApp2;

public class AppRepository(AppDbContext context)
{
    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
        return customer;
    }

    public Task<Customer?> GetCustomerAsync(int id)
    {
        return context.Customers
            .FirstOrDefaultAsync(customer => customer.Id == id);
    }

    public Task<Customer?> GetCustomerWithOrdersAsync(int id)
    {
        return context.Customers
            .Include(customer => customer.Orders)
            .ThenInclude(order => order.Items)
            .FirstOrDefaultAsync(customer => customer.Id == id);
    }

    public Task<List<Customer>> GetAllCustomersAsync()
    {
        return context.Customers.ToListAsync();
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        var existingCustomer = await context.Customers.FindAsync(customer.Id);
        if (existingCustomer is null)
        {
            return false;
        }

        existingCustomer.Name = customer.Name;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer is null)
        {
            return false;
        }

        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        return order;
    }

    public Task<Order?> GetOrderAsync(int id)
    {
        return context.Orders
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id);
    }

    public Task<List<Order>> GetAllOrdersAsync()
    {
        return context.Orders.ToListAsync();
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        var existingOrder = await context.Orders.FindAsync(order.Id);
        if (existingOrder is null)
        {
            return false;
        }

        existingOrder.Status = order.Status;
        existingOrder.CustomerId = order.CustomerId;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order is null)
        {
            return false;
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<OrderItem> CreateOrderItemAsync(OrderItem item)
    {
        context.OrderItems.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public Task<OrderItem?> GetOrderItemAsync(int id)
    {
        return context.OrderItems
            .FirstOrDefaultAsync(item => item.Id == id);
    }

    public Task<List<OrderItem>> GetAllOrderItemsAsync()
    {
        return context.OrderItems.ToListAsync();
    }

    public async Task<bool> UpdateOrderItemAsync(OrderItem item)
    {
        var existingItem = await context.OrderItems.FindAsync(item.Id);
        if (existingItem is null)
        {
            return false;
        }

        existingItem.ProductName = item.ProductName;
        existingItem.Quantity = item.Quantity;
        existingItem.OrderId = item.OrderId;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOrderItemAsync(int id)
    {
        var item = await context.OrderItems.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        context.OrderItems.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}
