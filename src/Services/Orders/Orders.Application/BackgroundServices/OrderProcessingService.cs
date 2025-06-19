using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;
namespace Orders.Application.BackgroundServices;

public class OrderProcessingService : BackgroundService
{
    private readonly ILogger<OrderProcessingService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly TimeSpan _processingInterval = TimeSpan.FromMinutes(5);

    public OrderProcessingService(
        ILogger<OrderProcessingService> logger,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Processing pending orders...");
                // In a real app, this would process payments, update inventory, etc.
                _logger.LogInformation("Pending orders processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing orders");
            }

            await Task.Delay(_processingInterval, stoppingToken);
        }
    }
}