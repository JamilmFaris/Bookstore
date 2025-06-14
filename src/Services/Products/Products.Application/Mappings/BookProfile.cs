using AutoMapper;
using Products.Application.DTOs;
using Products.Domain.Entities;

namespace Products.Application.Mappings;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<BookDto, Book>();
    }
}