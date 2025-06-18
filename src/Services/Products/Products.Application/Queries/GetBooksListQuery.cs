using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Queries;

public record GetBooksListQuery : IRequest<IEnumerable<BookDto>>;