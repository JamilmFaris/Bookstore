// Products.Application/Handlers/AddBookCommandHandler.cs
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Domain.Enums;
using Products.Application.Commands;
using MediatR;

public class AddBookCommandHandler : IRequestHandler<AddBookCommand, int>
{
    private readonly IBookRepository _repository;

    public AddBookCommandHandler(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(AddBookCommand request, CancellationToken ct)
    {
        var book = new Book
        {
            Title = request.Title,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Category = Enum.Parse<BookCategory>(request.Category),
            Author = request.Author,
            Price = request.Price,
            Status = BookStatus.Available // Default status
        };

        await _repository.AddAsync(book);
        return book.Id;
    }
}
