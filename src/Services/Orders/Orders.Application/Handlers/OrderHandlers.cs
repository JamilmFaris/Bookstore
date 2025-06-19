using AutoMapper;
using MediatR;
using Orders.Application.Commands;
using Orders.Application.DTOs;
using Orders.Domain.Entities;
using Orders.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BookOrderDto>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BookOrderDto> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = new BookOrder
        {
            UserId = request.Dto.UserId,
            ShippingAddress = request.Dto.ShippingAddress,
            TotalPrice = request.Dto.Items.Sum(item => item.Price * item.Quantity),
            Items = request.Dto.Items.Select(item => new OrderItem
            {
                BookId = item.BookId,
                BookTitle = item.BookTitle,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };

        await _repository.AddOrderAsync(order);
        return _mapper.Map<BookOrderDto>(order);
    }
}

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, BookOrderDto>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public UpdateOrderStatusCommandHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BookOrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
    {
        var order = await _repository.GetOrderByIdAsync(request.Dto.OrderId);
        if (order == null) throw new Exception("Order not found");

        if (Enum.TryParse<OrderStatus>(request.Dto.NewStatus, out var newStatus))
        {
            order.Status = newStatus;
            await _repository.UpdateOrderAsync(order);
        }
        else
        {
            throw new Exception("Invalid order status");
        }

        return _mapper.Map<BookOrderDto>(order);
    }
}

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Unit>
{
    private readonly IOrderRepository _repository;

    public CancelOrderCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken ct)
    {
        await _repository.CancelOrderAsync(request.OrderId);
        return Unit.Value;
    }
}