using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.Domain.Entities;
namespace Orders.Domain.Interfaces;

public interface IOrderRepository
{
    Task<BookOrder> GetOrderByIdAsync(Guid id);
    Task<IEnumerable<BookOrder>> GetOrdersByUserIdAsync(Guid userId);
    Task AddOrderAsync(BookOrder order);
    Task UpdateOrderAsync(BookOrder order);
    Task CancelOrderAsync(Guid orderId);
}