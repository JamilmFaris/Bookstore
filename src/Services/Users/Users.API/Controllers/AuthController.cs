using Microsoft.AspNetCore.Mvc;
using MediatR;
using Users.Application.Commands;
using Users.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Users.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterUserDto dto)
    {
        var response = await _mediator.Send(new RegisterUserCommand(dto));
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginDto dto)
    {
        var response = await _mediator.Send(new LoginUserCommand(dto));
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _mediator.Send(new LogoutCommand(token));
        return NoContent();
    }
}