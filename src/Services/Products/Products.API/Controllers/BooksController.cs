using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Queries;
using Products.Application.DTOs;

namespace Products.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var books = await _mediator.Send(new GetBooksListQuery());
        return Ok(books);
    }
}