// Products.Domain/Entities/Book.cs
using Products.Domain.Enums;

namespace Products.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public BookStatus Status { get; set; }
}
