using MediatR;
using Orders.Application.DTOs;

namespace Orders.Application.Commands;

public record CreateSubscriptionCommand(CreateSubscriptionDto Dto) : IRequest<SubscriptionDto>;