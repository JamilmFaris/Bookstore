using Microsoft.EntityFrameworkCore;
using Orders.Application.BackgroundServices;
using Orders.Domain.Interfaces;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AutoMapper;
var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

// Register repositories
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Add MediatR and AutoMapper
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<CreateSubscriptionCommandHandler>());
builder.Services.AddAutoMapper(typeof(Program));

// Background services
builder.Services.AddScoped<SubscriptionNotificationService>();
builder.Services.AddScoped<OrderProcessingService>();
builder.Services.AddHostedService(provider => 
    provider.GetRequiredService<SubscriptionNotificationService>());
builder.Services.AddHostedService(provider => 
    provider.GetRequiredService<OrderProcessingService>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    
    // Apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        db.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();