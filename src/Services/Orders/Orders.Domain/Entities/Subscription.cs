using System;
namespace Orders.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public SubscriptionType Type { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal Price { get; set; }
}

public enum SubscriptionType
{
    Monthly = 1,
    Yearly = 12
}