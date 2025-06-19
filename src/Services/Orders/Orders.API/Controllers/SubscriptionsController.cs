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
[Route("api/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<SubscriptionDto>> CreateSubscription(CreateSubscriptionDto dto)
    {
        var subscription = await _mediator.Send(new CreateSubscriptionCommand(dto));
        return Ok(subscription);
    }

    [HttpPost("renew")]
    public async Task<ActionResult<SubscriptionDto>> RenewSubscription(RenewSubscriptionDto dto)
    {
        var subscription = await _mediator.Send(new RenewSubscriptionCommand(dto));
        return Ok(subscription);
    }
}