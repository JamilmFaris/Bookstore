using MediatR;
using Users.Application.DTOs;

namespace Users.Application.Queries;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;