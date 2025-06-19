using MediatR;
using Orders.Application.DTOs;
using System;
namespace Orders.Application.Commands;

public record CreateOrderCommand(CreateOrderDto Dto) : IRequest<BookOrderDto>;
public record UpdateOrderStatusCommand(UpdateOrderStatusDto Dto) : IRequest<BookOrderDto>;
public record CancelOrderCommand(Guid OrderId) : IRequest<Unit>;