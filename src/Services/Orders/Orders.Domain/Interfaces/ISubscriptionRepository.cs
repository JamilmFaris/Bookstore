using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.Domain.Entities;

namespace Orders.Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription> GetByIdAsync(Guid id);
    Task<Subscription> GetActiveSubscriptionAsync(Guid userId);
    Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(int daysBeforeExpiry);
    Task AddAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
}