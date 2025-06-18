using MediatR;
using Users.Application.DTOs;

namespace Users.Application.Commands;

public record UpdateSubscriptionCommand(
    Guid UserId,
    SubscriptionDto Dto) : IRequest<UserDto>;