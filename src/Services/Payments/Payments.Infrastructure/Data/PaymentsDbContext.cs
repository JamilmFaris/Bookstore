using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Payments.Infrastructure.Data
{
    public class PaymentsDbContext : DbContext
    {
        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options)
            : base(options) { }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.UserId);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.SubscriptionId);
        }
    }
}