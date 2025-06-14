using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Queries;

public class GetBooksListQuery : IRequest<IEnumerable<BookDto>> { }