using BolaoCopaApp.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BolaoCopaApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TournamentController : ControllerBase
{
    private readonly IMediator _mediator;

    public TournamentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("phase")]
    public async Task<ActionResult<string>> GetPhase()
    {
        var phase = await _mediator.Send(new GetTournamentPhaseQuery());
        return Ok(phase);
    }
}
