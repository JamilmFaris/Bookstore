
using MediatR;
using Users.Application.DTOs;

namespace Users.Application.Commands;

public record UpdateUserCommand(
    Guid UserId,
    UpdateUserDto Dto) : IRequest<UserDto>;