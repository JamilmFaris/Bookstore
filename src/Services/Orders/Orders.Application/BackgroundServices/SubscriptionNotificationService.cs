using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Orders.Application.BackgroundServices;

public class SubscriptionNotificationService : BackgroundService
{
    private readonly ILogger<SubscriptionNotificationService> _logger;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Run daily

    public SubscriptionNotificationService(
        ILogger<SubscriptionNotificationService> logger,
        ISubscriptionRepository subscriptionRepository)
    {
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Checking for expiring subscriptions...");
                var expiringSubscriptions = await _subscriptionRepository
                    .GetExpiringSubscriptionsAsync(3); // Notify 3 days before expiry

                foreach (var subscription in expiringSubscriptions)
                {
                    // In a real app: Send email/push notification
                    _logger.LogWarning($"Subscription {subscription.Id} for user {subscription.UserId} expires on {subscription.EndDate:yyyy-MM-dd}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking expiring subscriptions");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}