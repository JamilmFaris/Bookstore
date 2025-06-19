using MediatR;
using Orders.Application.DTOs;

namespace Orders.Application.Commands;

public record RenewSubscriptionCommand(RenewSubscriptionDto Dto) : IRequest<SubscriptionDto>;