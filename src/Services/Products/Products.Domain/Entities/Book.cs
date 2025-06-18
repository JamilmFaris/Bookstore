// Products.Domain/Entities/Book.cs
using Products.Domain.Enums;

namespace Products.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }          // Name of the book
    public string Description { get; set; }    // New property
    public string ImageUrl { get; set; }       // New property
    public BookCategory Category { get; set; }  // eBook or audioBook
    public string Author { get; set; }
    public decimal Price { get; set; }
    public BookStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // New property
}