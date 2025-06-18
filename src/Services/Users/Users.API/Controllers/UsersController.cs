using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Users.Application.Commands;
using Users.Application.DTOs;
using Users.Application.Queries;

namespace Users.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid id, UpdateUserDto dto)
    {
        var user = await _mediator.Send(new UpdateUserCommand(id, dto));
        return Ok(user);
    }

    [HttpPut("{id}/subscription")]
    public async Task<ActionResult<UserDto>> UpdateSubscription(Guid id, SubscriptionDto dto)
    {
        var user = await _mediator.Send(new UpdateSubscriptionCommand(id, dto));
        return Ok(user);
    }

    [HttpGet("{id}/subscription")]
    public async Task<ActionResult<bool>> CheckSubscription(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user.IsSubscribed && user.SubscriptionExpiry > DateTime.UtcNow);
    }
}