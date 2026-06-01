using BolaoCopaApp.Application.Queries;
using BolaoCopaApp.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BolaoCopaApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatches([FromQuery] string? group, [FromQuery] string? round)
    {
        var result = await _mediator.Send(new GetMatchesQuery(group, round));
        return Ok(result);
    }
}
