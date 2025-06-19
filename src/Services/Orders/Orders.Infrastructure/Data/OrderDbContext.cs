using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Orders.Domain.Entities;
namespace Orders.Infrastructure.Data;

public class OrderDbContext : DbContext
{
    public DbSet<BookOrder> BookOrders { get; set; }
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Notification> Notifications => Set<Notification>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>()
            .HasIndex(s => s.UserId);
            
        modelBuilder.Entity<Notification>()
            .HasIndex(n => n.UserId);
    }
}

public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<OrderDbContext>();
        builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new OrderDbContext(builder.Options);
    }
}