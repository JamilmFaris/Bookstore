using MediatR;

namespace Users.Application.Commands;

public record LogoutCommand(string Token) : IRequest<Unit>;