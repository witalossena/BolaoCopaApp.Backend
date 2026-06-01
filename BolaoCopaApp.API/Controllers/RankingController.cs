using BolaoCopaApp.Application.Queries;
using BolaoCopaApp.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BolaoCopaApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RankingController : ControllerBase
{
    private readonly IMediator _mediator;

    public RankingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RankingEntryDto>>> GetRanking()
    {
        var result = await _mediator.Send(new GetRankingQuery());
        return Ok(result);
    }
}
