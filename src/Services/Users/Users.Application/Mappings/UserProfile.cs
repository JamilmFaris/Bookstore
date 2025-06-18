using AutoMapper;
using Users.Domain.Entities;
using Users.Application.DTOs;

namespace Users.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.IsSubscribed, opt => opt.MapFrom(src => src.IsSubscribed))
            .ForMember(dest => dest.SubscriptionExpiry, opt => opt.MapFrom(src => src.SubscriptionExpiry));
    }
}