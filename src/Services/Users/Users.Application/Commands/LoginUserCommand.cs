using MediatR;
using Users.Application.DTOs;

namespace Users.Application.Commands;

public record LoginUserCommand(LoginDto Dto) : IRequest<AuthResponse>;