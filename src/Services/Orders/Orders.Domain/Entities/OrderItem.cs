using System;
namespace Orders.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid OrderId { get; set; }
    public BookOrder Order { get; set; } = null!;
}