using BolaoCopaApp.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolaoCopaApp.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")] // Just a stub for Admin role
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("match-result")]
    public async Task<ActionResult> RegisterMatchResult([FromBody] RegisterMatchResultCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Match result registered." });
    }

    [HttpPatch("users/{id}/payment")]
    public async Task<ActionResult> ToggleUserPayment(Guid id, [FromBody] bool isPaid)
    {
        await _mediator.Send(new ToggleUserPaymentCommand(id, isPaid));
        return Ok(new { message = "Payment status updated." });
    }
}
