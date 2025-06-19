using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces;
using Orders.Infrastructure.Data;
using System;          // For Guid
using System.Collections.Generic; // For IEnumerable<T>
using System.Threading.Tasks; // For Task
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Orders.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly OrderDbContext _context;

    public SubscriptionRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription> GetByIdAsync(Guid id)
        => await _context.Subscriptions.FindAsync(id);

    public async Task<Subscription> GetActiveSubscriptionAsync(Guid userId)
        => await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive);

    public async Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(int daysBeforeExpiry)
        => await _context.Subscriptions
            .Where(s => s.IsActive && s.EndDate <= DateTime.UtcNow.AddDays(daysBeforeExpiry))
            .ToListAsync();

    public async Task AddAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
    }
}