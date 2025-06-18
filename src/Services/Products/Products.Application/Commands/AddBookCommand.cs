// Products.Application/Commands/AddBookCommand.cs
using MediatR;

namespace Products.Application.Commands;

public record AddBookCommand(
    string Title,
    string Description,
    string ImageUrl,
    string Category,
    string Author,
    decimal Price) : IRequest<int>;


