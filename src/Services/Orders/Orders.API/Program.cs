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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

// Register repositories
builder.Services.AddScoped<SubscriptionNotificationService>();
builder.Services.AddScoped<OrderProcessingService>();
builder.Services.AddHostedService(provider => 
    provider.GetRequiredService<SubscriptionNotificationService>());
builder.Services.AddHostedService(provider => 
    provider.GetRequiredService<OrderProcessingService>());

// Add MediatR and AutoMapper
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<CreateSubscriptionCommandHandler>());
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
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

var jwtSecret = builder.Configuration["JwtSettings:Secret"];

if (string.IsNullOrEmpty(jwtSecret))
    throw new Exception("JWT Secret is missing in configuration");

var key = Encoding.UTF8.GetBytes(jwtSecret);

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,    // Must match Users service
            ValidateAudience = false   // Must match Users service
        };
    });

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();