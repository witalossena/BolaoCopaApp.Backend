using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BolaoCopaApp.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] // Requires login
public class PredictionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PredictionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString()); // Fallback for dummy tests

    [HttpGet("me")]
    public async Task<ActionResult<UserPredictionsDto>> GetMyPredictions()
    {
        var result = await _mediator.Send(new GetUserPredictionsQuery(GetUserId()));
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<PredictionHistoryItemDto>>> GetHistory()
    {
        var result = await _mediator.Send(new GetUserHistoryQuery(GetUserId()));
        return Ok(result);
    }

    [HttpPost("match")]
    public async Task<ActionResult> SubmitMatchPrediction([FromBody] PredictionDto request)
    {
        try
        {
            await _mediator.Send(new SubmitPredictionCommand(GetUserId(), request.MatchId, request.HomeScore, request.AwayScore));
            return Ok(new { message = "Prediction saved." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("group-rank")]
    public async Task<ActionResult> SubmitGroupRankPrediction([FromBody] GroupRankDto request)
    {
        try
        {
            await _mediator.Send(new SubmitGroupRankCommand(GetUserId(), request.Group, request.FirstTeam, request.SecondTeam, request.ThirdTeam, request.FourthTeam));
            return Ok(new { message = "Group rank prediction saved." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("knockout")]
    public async Task<ActionResult> ClearKnockoutPredictions()
    {
        try
        {
            await _mediator.Send(new ClearKnockoutPredictionsCommand(GetUserId()));
            return Ok(new { message = "Knockout predictions cleared." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("knockout")]
    public async Task<ActionResult> SubmitKnockoutPrediction([FromBody] KnockoutPredictionDto request)
    {
        try
        {
            await _mediator.Send(new SubmitKnockoutPredictionCommand(GetUserId(), request.MatchId, request.WinnerTeam, request.HomeScore, request.AwayScore));
            return Ok(new { message = "Knockout prediction saved." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
