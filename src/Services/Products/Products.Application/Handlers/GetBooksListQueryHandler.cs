// Products.Application/Handlers/GetBooksListQueryHandler.cs
using AutoMapper;
using MediatR;
using Products.Application.DTOs;
using Products.Application.Queries;
using Products.Domain.Interfaces;

public class GetBooksListQueryHandler : IRequestHandler<GetBooksListQuery, IEnumerable<BookDto>>
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public GetBooksListQueryHandler(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> Handle(
        GetBooksListQuery request, 
        CancellationToken ct)
    {
        var books = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }
}