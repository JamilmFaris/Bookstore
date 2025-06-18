// Products.API/Controllers/BooksController.cs
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Products.Application.DTOs;
using Products.Application.Commands;
using Products.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator) => _mediator = mediator;

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var books = await _mediator.Send(new GetBooksListQuery());
        return Ok(books);
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<int>> AddBook(CreateBookDto dto)
    {
        var bookId = await _mediator.Send(new AddBookCommand(
            dto.Title,
            dto.Description,
            dto.ImageUrl,
            dto.Category,
            dto.Author,
            dto.Price));

        return CreatedAtAction(nameof(GetBooks), new { id = bookId }, bookId);
    }
}