using System;
using System.Collections.Generic;
namespace Orders.Application.DTOs;

public record OrderItemDto(
    Guid BookId,
    string BookTitle,
    decimal Price,
    int Quantity);

public record BookOrderDto(
    Guid Id,
    Guid UserId,
    DateTime OrderDate,
    string Status,
    decimal TotalPrice,
    List<OrderItemDto> Items);

public record CreateOrderDto(
    Guid UserId,
    List<OrderItemDto> Items,
    string ShippingAddress);

public record UpdateOrderStatusDto(
    Guid OrderId,
    string NewStatus);