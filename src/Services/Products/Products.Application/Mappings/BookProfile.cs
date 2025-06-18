// Products.Application/Mappings/BookProfile.cs
using AutoMapper;
using Products.Domain.Entities;
using Products.Application.DTOs;
using Products.Domain.Enums;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Category, 
                       opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Status,
                       opt => opt.MapFrom(src => src.Status.ToString()));
    }
}