// Products.Application/DTOs/BookDto.cs
namespace Products.Application.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; } // Will be string like "eBook" or "audioBook"
    public string Author { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
}

public class CreateBookDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
}