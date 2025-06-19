using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces;
using Orders.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace Orders.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<BookOrder> GetOrderByIdAsync(Guid id)
        => await _context.BookOrders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<BookOrder>> GetOrdersByUserIdAsync(Guid userId)
        => await _context.BookOrders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync();

    public async Task AddOrderAsync(BookOrder order)
    {
        await _context.BookOrders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(BookOrder order)
    {
        _context.BookOrders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task CancelOrderAsync(Guid orderId)
    {
        var order = await _context.BookOrders.FindAsync(orderId);
        if (order != null && order.Status == OrderStatus.Pending)
        {
            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();
        }
    }
}