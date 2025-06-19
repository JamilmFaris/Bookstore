using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Payments.Domain.Interfaces;
using Payments.Infrastructure.Data;
using Payments.Infrastructure.Repositories;
using System;
using Payments.Application.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(ProcessPayment.Command).Assembly));

// Add DbContext
builder.Services.AddDbContext<PaymentsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

// Register repositories
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
// Configure middleware
if (app.Environment.IsDevelopment())
{    
    // Apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
        db.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();