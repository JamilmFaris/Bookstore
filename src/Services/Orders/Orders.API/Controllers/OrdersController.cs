using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands;
using Orders.Application.DTOs;
using System;
using System.Threading.Tasks; 
namespace Orders.API.Controllers;

[Authorize]
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<BookOrderDto>> CreateOrder(CreateOrderDto dto)
    {
        var order = await _mediator.Send(new CreateOrderCommand(dto));
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookOrderDto>> GetOrder(Guid id)
    {
        // Implementation would require a query handler
        return Ok();
    }

    [HttpPut("status")]
    public async Task<ActionResult<BookOrderDto>> UpdateOrderStatus(UpdateOrderStatusDto dto)
    {
        var order = await _mediator.Send(new UpdateOrderStatusCommand(dto));
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        await _mediator.Send(new CancelOrderCommand(id));
        return NoContent();
    }
}