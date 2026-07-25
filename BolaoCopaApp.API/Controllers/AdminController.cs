using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Application.Queries;
using ClosedXML.Excel;
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

    [HttpPatch("matches/{id}/live-score")]
    public async Task<ActionResult> UpdateLiveScore(string id, [FromBody] LiveScoreDto dto)
    {
        try
        {
            await _mediator.Send(new UpdateLiveScoreCommand(id, dto.HomeScore, dto.AwayScore));
            return Ok(new { message = "Live score updated." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("match-result")]
    public async Task<ActionResult> RegisterMatchResult([FromBody] MatchResultRequestDto dto)
    {
        try
        {
            await _mediator.Send(new RegisterMatchResultCommand(dto.MatchId, dto.HomeScore, dto.AwayScore, dto.Resolution, dto.WinnerTeam));
            return Ok(new { message = "Match result registered." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("matches/{id}/result")]
    public async Task<ActionResult> ResetMatchResult(string id)
    {
        try
        {
            await _mediator.Send(new ResetMatchResultCommand(id));
            return Ok(new { message = "Match result reset." });
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

    [HttpPatch("users/{id}/unlock-predictions")]
    public async Task<ActionResult> ToggleUserPredictionUnlock(Guid id, [FromBody] bool isUnlocked)
    {
        try
        {
            await _mediator.Send(new ToggleUserPredictionUnlockCommand(id, isUnlocked));
            return Ok(new { message = isUnlocked ? "User predictions unlocked." : "User predictions lock restored." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("group-results")]
    public async Task<ActionResult<IEnumerable<GroupResultSummaryDto>>> GetGroupResults()
    {
        var results = await _mediator.Send(new GetGroupResultsQuery());
        return Ok(results);
    }

    [HttpPost("group-result")]
    public async Task<ActionResult> SetGroupResult([FromBody] GroupResultDto dto)
    {
        try
        {
            await _mediator.Send(new SetGroupResultCommand(dto.Group, dto.FirstTeam, dto.SecondTeam, dto.ThirdTeam, dto.FourthTeam));
            return Ok(new { message = "Group result saved." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("group-result/{group}")]
    public async Task<ActionResult> ResetGroupResult(string group)
    {
        await _mediator.Send(new ResetGroupResultCommand(group));
        return Ok(new { message = "Group result reset." });
    }

    [HttpPatch("tournament/prize-pool")]
    public async Task<ActionResult> SetPrizePool([FromBody] decimal amount)
    {
        try
        {
            await _mediator.Send(new SetPrizePoolCommand(amount));
            return Ok(new { message = "Prize pool updated." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("tournament/phase")]
    public async Task<ActionResult> SetTournamentPhase([FromBody] string phase)
    {
        try
        {
            await _mediator.Send(new SetTournamentPhaseCommand(phase));
            return Ok(new { message = $"Phase set to {phase}." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("tournament/lock-predictions")]
    public async Task<ActionResult> LockAllPredictions([FromBody] bool isLocked)
    {
        try
        {
            await _mediator.Send(new LockAllPredictionsCommand(isLocked));
            return Ok(new { message = isLocked ? "All predictions locked." : "All predictions unlocked." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("confirm-payment")]
    public async Task<ActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
    {
        try
        {
            await _mediator.Send(new ConfirmPaymentCommand(request.Handle, request.Amount));
            return Ok(new { message = "Payment confirmed and prize pool updated." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("calculate-group-scores")]
    public async Task<ActionResult> CalculateGroupScores()
    {
        await _mediator.Send(new CalculateGroupRankScoresCommand());
        return Ok(new { message = "Group rank scores calculated." });
    }

    [HttpPost("calculate-knockout-scores")]
    public async Task<ActionResult> CalculateKnockoutScores()
    {
        await _mediator.Send(new CalculateKnockoutScoresCommand());
        return Ok(new { message = "Knockout scores calculated." });
    }

    [HttpGet("report/excel")]
    public async Task<ActionResult> DownloadReport()
    {
        var data = await _mediator.Send(new GetBetsReportQuery());

        using var wb = new XLWorkbook();

        // Sheet 1: Resumo
        var ws1 = wb.Worksheets.Add("Resumo");
        var headers1 = new[] { "Nome", "Handle", "Pts Partidas", "Pts Grupos", "Pts Mata-Mata", "Pts Especiais", "Total" };
        for (int i = 0; i < headers1.Length; i++) ws1.Cell(1, i + 1).Value = headers1[i];
        var users = data.Users.OrderByDescending(u => u.Total).ToList();
        for (int r = 0; r < users.Count; r++)
        {
            var u = users[r];
            ws1.Cell(r + 2, 1).Value = u.Name;
            ws1.Cell(r + 2, 2).Value = u.Handle;
            ws1.Cell(r + 2, 3).Value = u.MatchPts;
            ws1.Cell(r + 2, 4).Value = u.GroupPts;
            ws1.Cell(r + 2, 5).Value = u.KnockoutPts;
            ws1.Cell(r + 2, 6).Value = u.SpecialPts;
            ws1.Cell(r + 2, 7).Value = u.Total;
        }
        int totalRow1 = users.Count + 2;
        ws1.Cell(totalRow1, 1).Value = "TOTAL PARTICIPANTES";
        ws1.Cell(totalRow1, 2).Value = users.Count;
        ws1.Range(1, 1, 1, 7).Style.Font.Bold = true;
        ws1.Columns().AdjustToContents();

        // Sheet 2: Partidas
        var ws2 = wb.Worksheets.Add("Partidas");
        var headers2 = new[] { "Nome", "Handle", "Casa", "Visitante", "Grupo", "Fase", "Palpite", "Resultado Real", "Pontos" };
        for (int i = 0; i < headers2.Length; i++) ws2.Cell(1, i + 1).Value = headers2[i];
        var mPreds = data.MatchPredictions.ToList();
        for (int r = 0; r < mPreds.Count; r++)
        {
            var p = mPreds[r];
            ws2.Cell(r + 2, 1).Value = p.UserName;
            ws2.Cell(r + 2, 2).Value = p.UserHandle;
            ws2.Cell(r + 2, 3).Value = p.HomeTeam;
            ws2.Cell(r + 2, 4).Value = p.AwayTeam;
            ws2.Cell(r + 2, 5).Value = p.Group;
            ws2.Cell(r + 2, 6).Value = p.Round;
            ws2.Cell(r + 2, 7).Value = p.Prediction;
            ws2.Cell(r + 2, 8).Value = p.RealResult;
            ws2.Cell(r + 2, 9).Value = p.Points;
        }
        int totalRow2 = mPreds.Count + 2;
        ws2.Cell(totalRow2, 1).Value = "TOTAL APOSTAS";
        ws2.Cell(totalRow2, 2).Value = mPreds.Count;
        ws2.Cell(totalRow2, 9).Value = mPreds.Sum(p => p.Points);
        ws2.Range(1, 1, 1, 9).Style.Font.Bold = true;
        ws2.Columns().AdjustToContents();

        // Sheet 3: Grupos
        var ws3 = wb.Worksheets.Add("Grupos");
        var headers3 = new[] { "Nome", "Handle", "Grupo", "1º Palpite", "2º Palpite", "3º Palpite", "4º Palpite", "1º Real", "2º Real", "3º Real", "4º Real", "Pontos" };
        for (int i = 0; i < headers3.Length; i++) ws3.Cell(1, i + 1).Value = headers3[i];
        var gPreds = data.GroupPredictions.ToList();
        for (int r = 0; r < gPreds.Count; r++)
        {
            var g = gPreds[r];
            ws3.Cell(r + 2, 1).Value = g.UserName;
            ws3.Cell(r + 2, 2).Value = g.UserHandle;
            ws3.Cell(r + 2, 3).Value = g.Group;
            ws3.Cell(r + 2, 4).Value = g.First;
            ws3.Cell(r + 2, 5).Value = g.Second;
            ws3.Cell(r + 2, 6).Value = g.Third ?? "";
            ws3.Cell(r + 2, 7).Value = g.Fourth ?? "";
            ws3.Cell(r + 2, 8).Value = g.RealFirst;
            ws3.Cell(r + 2, 9).Value = g.RealSecond;
            ws3.Cell(r + 2, 10).Value = g.RealThird ?? "";
            ws3.Cell(r + 2, 11).Value = g.RealFourth ?? "";
            ws3.Cell(r + 2, 12).Value = g.Points;
        }
        int totalRow3 = gPreds.Count + 2;
        ws3.Cell(totalRow3, 1).Value = "TOTAL APOSTAS";
        ws3.Cell(totalRow3, 2).Value = gPreds.Count;
        ws3.Cell(totalRow3, 12).Value = gPreds.Sum(g => g.Points);
        ws3.Range(1, 1, 1, 12).Style.Font.Bold = true;
        ws3.Columns().AdjustToContents();

        // Sheet 4: Mata-Mata
        var ws4 = wb.Worksheets.Add("Mata-Mata");
        var headers4 = new[] { "Nome", "Handle", "Casa", "Visitante", "Fase", "Palpite Vencedor", "Prorrog./Pênalti (Palpite)", "Vencedor Real", "Prorrog./Pênalti (Real)", "Pontos" };
        for (int i = 0; i < headers4.Length; i++) ws4.Cell(1, i + 1).Value = headers4[i];
        var kPreds = data.KnockoutPredictions.ToList();
        for (int r = 0; r < kPreds.Count; r++)
        {
            var k = kPreds[r];
            ws4.Cell(r + 2, 1).Value = k.UserName;
            ws4.Cell(r + 2, 2).Value = k.UserHandle;
            ws4.Cell(r + 2, 3).Value = k.HomeTeam;
            ws4.Cell(r + 2, 4).Value = k.AwayTeam;
            ws4.Cell(r + 2, 5).Value = k.Round;
            ws4.Cell(r + 2, 6).Value = k.WinnerTeam;
            ws4.Cell(r + 2, 7).Value = k.Resolution ?? "";
            ws4.Cell(r + 2, 8).Value = k.RealWinner;
            ws4.Cell(r + 2, 9).Value = k.RealResolution ?? "";
            ws4.Cell(r + 2, 10).Value = k.Points;
        }
        int totalRow4 = kPreds.Count + 2;
        ws4.Cell(totalRow4, 1).Value = "TOTAL APOSTAS";
        ws4.Cell(totalRow4, 2).Value = kPreds.Count;
        ws4.Cell(totalRow4, 10).Value = kPreds.Sum(k => k.Points);
        ws4.Range(1, 1, 1, 10).Style.Font.Bold = true;
        ws4.Columns().AdjustToContents();

        // Sheet 5: Especiais
        var ws5 = wb.Worksheets.Add("Especiais");
        var headers5 = new[] { "Nome", "Handle", "Campeão", "Vice", "3º Lugar", "4º Lugar", "Artilheiro", "Assistências", "MVP", "Revelação", "Pontos" };
        for (int i = 0; i < headers5.Length; i++) ws5.Cell(1, i + 1).Value = headers5[i];
        var sPreds = data.SpecialPredictions.ToList();
        for (int r = 0; r < sPreds.Count; r++)
        {
            var s = sPreds[r];
            ws5.Cell(r + 2, 1).Value = s.UserName;
            ws5.Cell(r + 2, 2).Value = s.UserHandle;
            ws5.Cell(r + 2, 3).Value = s.Champion ?? "";
            ws5.Cell(r + 2, 4).Value = s.RunnerUp ?? "";
            ws5.Cell(r + 2, 5).Value = s.ThirdPlace ?? "";
            ws5.Cell(r + 2, 6).Value = s.OtherFinalist ?? "";
            ws5.Cell(r + 2, 7).Value = s.TopScorer ?? "";
            ws5.Cell(r + 2, 8).Value = s.MostAssists ?? "";
            ws5.Cell(r + 2, 9).Value = s.MVP ?? "";
            ws5.Cell(r + 2, 10).Value = s.GoldenBoy ?? "";
            ws5.Cell(r + 2, 11).Value = s.Points;
        }
        int totalRow5 = sPreds.Count + 2;
        ws5.Cell(totalRow5, 1).Value = "TOTAL PARTICIPANTES";
        ws5.Cell(totalRow5, 2).Value = sPreds.Count;
        ws5.Cell(totalRow5, 11).Value = sPreds.Sum(s => s.Points);
        ws5.Range(1, 1, 1, 11).Style.Font.Bold = true;
        ws5.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);

        var fileName = $"relatorio_bolao_{DateTime.UtcNow:yyyyMMdd_HHmm}.xlsx";
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
