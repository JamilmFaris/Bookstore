// Products.Infrastructure/Data/BookstoreContext.cs
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infrastructure.Data;

public class BookstoreContext : DbContext
{
    public BookstoreContext(DbContextOptions<BookstoreContext> options)
        : base(options) { }

    public DbSet<Book> Books => Set<Book>();
}