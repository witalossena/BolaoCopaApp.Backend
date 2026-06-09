using BolaoCopaApp.Application.DTOs;
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

    [HttpGet("info")]
    public async Task<ActionResult<TournamentInfoDto>> GetInfo()
    {
        var info = await _mediator.Send(new GetTournamentInfoQuery());
        return Ok(info);
    }
}
