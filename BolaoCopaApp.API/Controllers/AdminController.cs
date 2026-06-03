using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Application.Queries;
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

    [HttpGet("stats")]
    public async Task<ActionResult<AdminStatsDto>> GetStats()
    {
        var stats = await _mediator.Send(new GetAdminStatsQuery());
        return Ok(stats);
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<AdminUserDto>>> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersAdminQuery());
        return Ok(users);
    }

    [HttpPost("match-result")]
    public async Task<ActionResult> RegisterMatchResult([FromBody] MatchResultRequestDto dto)
    {
        try
        {
            await _mediator.Send(new RegisterMatchResultCommand(dto.MatchId, dto.HomeScore, dto.AwayScore));
            return Ok(new { message = "Match result registered." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("matches/{id}/teams")]
    public async Task<ActionResult> UpdateMatchTeams(string id, [FromBody] UpdateMatchTeamsDto dto)
    {
        await _mediator.Send(new UpdateMatchTeamsCommand(id, dto.HomeTeam, dto.AwayTeam));
        return Ok(new { message = "Match teams updated." });
    }

    [HttpPatch("matches/{id}/lock")]
    public async Task<ActionResult> LockMatch(string id, [FromBody] bool isLocked)
    {
        await _mediator.Send(new LockMatchCommand(id, isLocked));
        return Ok(new { message = "Match lock status updated." });
    }

    [HttpPost("calculate-scores")]
    public async Task<ActionResult> CalculateScores()
    {
        await _mediator.Send(new CalculateAllScoresCommand());
        return Ok(new { message = "Scores calculated." });
    }

    [HttpGet("users/{id}/predictions")]
    public async Task<ActionResult<UserPredictionsDto>> GetUserPredictions(Guid id)
    {
        var predictions = await _mediator.Send(new GetUserPredictionsQuery(id));
        return Ok(predictions);
    }

    [HttpPatch("users/{id}/payment")]
    public async Task<ActionResult> ToggleUserPayment(Guid id, [FromBody] bool isPaid)
    {
        await _mediator.Send(new ToggleUserPaymentCommand(id, isPaid));
        return Ok(new { message = "Payment status updated." });
    }
}
