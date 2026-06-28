using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Application.Queries;
using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using BolaoCopaApp.Domain.Services;
using MediatR;
using BolaoCopaApp.Domain.Enums;

namespace BolaoCopaApp.Application.Handlers;

public class AdminHandlers :
    IRequestHandler<UpdateLiveScoreCommand, bool>,
    IRequestHandler<RegisterMatchResultCommand, bool>,
    IRequestHandler<ResetMatchResultCommand, bool>,
    IRequestHandler<ToggleUserPaymentCommand, bool>,
    IRequestHandler<CalculateAllScoresCommand, bool>,
    IRequestHandler<CalculateKnockoutScoresCommand, bool>,
    IRequestHandler<LockMatchCommand, bool>,
    IRequestHandler<UpdateMatchTeamsCommand, bool>,
    IRequestHandler<SetGroupResultCommand, bool>,
    IRequestHandler<ResetGroupResultCommand, bool>,
    IRequestHandler<CalculateGroupRankScoresCommand, bool>,
    IRequestHandler<SetTournamentPhaseCommand, bool>,
    IRequestHandler<SetPrizePoolCommand, bool>,
    IRequestHandler<LockAllPredictionsCommand, bool>,
    IRequestHandler<ToggleUserPredictionUnlockCommand, bool>,
    IRequestHandler<ConfirmPaymentCommand, bool>,
    IRequestHandler<GetGroupResultsQuery, IEnumerable<GroupResultSummaryDto>>
{
    private readonly IMatchRepository _matchRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPredictionRepository _predictionRepo;
    private readonly IGroupRankPredictionRepository _groupRankRepo;
    private readonly IGroupResultRepository _groupResultRepo;
    private readonly ITournamentRepository _tournamentRepo;
    private readonly IKnockoutPredictionRepository _knockoutRepo;
    private readonly ScoringService _scoringService;
    private readonly IUnitOfWork _uow;

    public AdminHandlers(
        IMatchRepository matchRepo,
        IUserRepository userRepo,
        IPredictionRepository predictionRepo,
        IGroupRankPredictionRepository groupRankRepo,
        IGroupResultRepository groupResultRepo,
        ITournamentRepository tournamentRepo,
        IKnockoutPredictionRepository knockoutRepo,
        ScoringService scoringService,
        IUnitOfWork uow)
    {
        _matchRepo = matchRepo;
        _userRepo = userRepo;
        _predictionRepo = predictionRepo;
        _groupRankRepo = groupRankRepo;
        _groupResultRepo = groupResultRepo;
        _tournamentRepo = tournamentRepo;
        _knockoutRepo = knockoutRepo;
        _scoringService = scoringService;
        _uow = uow;
    }

    public async Task<bool> Handle(UpdateLiveScoreCommand request, CancellationToken cancellationToken)
    {
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");
        if (match.Status == MatchStatus.Locked) throw new InvalidOperationException("Cannot update score of a finalized match.");

        match.HomeScore = request.HomeScore;
        match.AwayScore = request.AwayScore;
        match.Status = MatchStatus.Live;

        _matchRepo.Update(match);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(RegisterMatchResultCommand request, CancellationToken cancellationToken)
    {
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");
        if (match.Status == MatchStatus.Locked) throw new InvalidOperationException("Cannot change score of a finalized match.");

        match.HomeScore = request.HomeScore;
        match.AwayScore = request.AwayScore;
        match.Resolution = request.Resolution;
        match.Status = MatchStatus.Locked;

        _matchRepo.Update(match);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(ResetMatchResultCommand request, CancellationToken cancellationToken)
    {
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");

        match.HomeScore = null;
        match.AwayScore = null;
        match.Status = MatchStatus.Open;

        var predictions = await _predictionRepo.GetByMatchIdAsync(match.Id, cancellationToken);
        foreach (var pred in predictions)
        {
            pred.Points = 0;
            _predictionRepo.Update(pred);
        }

        _matchRepo.Update(match);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(ToggleUserPaymentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) throw new Exception("User not found");

        user.IsPaid = request.IsPaid;
        _userRepo.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(CalculateAllScoresCommand request, CancellationToken cancellationToken)
    {
        var allMatches = await _matchRepo.GetAllAsync(cancellationToken);
        var lockedMatches = allMatches
            .Where(m => (m.Status == MatchStatus.Locked || m.Status == MatchStatus.Live) && m.HomeScore != null && m.AwayScore != null)
            .ToList();

        foreach (var match in lockedMatches)
        {
            var predictions = await _predictionRepo.GetByMatchIdAsync(match.Id, cancellationToken);
            foreach (var pred in predictions)
            {
                pred.Points = _scoringService.CalculateMatchScore(
                    pred.HomeScore,
                    pred.AwayScore,
                    match.HomeScore!,
                    match.AwayScore!
                );
                _predictionRepo.Update(pred);
            }
        }

        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(CalculateKnockoutScoresCommand request, CancellationToken cancellationToken)
    {
        var allMatches = await _matchRepo.GetAllAsync(cancellationToken);
        var knockoutMatches = allMatches
            .Where(m => m.ExternalId.StartsWith("ko_") && m.Status == MatchStatus.Locked && m.HomeScore != null && m.AwayScore != null)
            .ToList();

        foreach (var match in knockoutMatches)
        {
            var winner = match.HomeScore!.Value > match.AwayScore!.Value ? match.HomeTeam : match.AwayTeam;
            var predictions = await _knockoutRepo.GetByMatchIdAsync(match.Id, cancellationToken);
            foreach (var pred in predictions)
            {
                pred.Points = _scoringService.CalculateKnockoutScore(
                    pred.WinnerTeam, pred.HomeScore, pred.AwayScore, pred.Resolution,
                    winner, match.HomeScore.Value, match.AwayScore.Value, match.Resolution);
                _knockoutRepo.Update(pred);
            }
        }

        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(LockMatchCommand request, CancellationToken cancellationToken)
    {
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");

        match.Status = request.IsLocked ? MatchStatus.Locked : MatchStatus.Open;
        _matchRepo.Update(match);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(UpdateMatchTeamsCommand request, CancellationToken cancellationToken)
    {
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");

        match.HomeTeam = request.HomeTeam;
        match.AwayTeam = request.AwayTeam;
        _matchRepo.Update(match);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(SetGroupResultCommand request, CancellationToken cancellationToken)
    {
        var existing = await _groupResultRepo.GetByGroupAsync(request.Group, cancellationToken);
        if (existing != null)
        {
            existing.FirstTeam = request.FirstTeam;
            existing.SecondTeam = request.SecondTeam;
            existing.ThirdTeam = request.ThirdTeam;
            existing.FourthTeam = request.FourthTeam;
            _groupResultRepo.Update(existing);
        }
        else
        {
            await _groupResultRepo.AddAsync(new GroupResult
            {
                Group = request.Group,
                FirstTeam = request.FirstTeam,
                SecondTeam = request.SecondTeam,
                ThirdTeam = request.ThirdTeam,
                FourthTeam = request.FourthTeam
            }, cancellationToken);
        }
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(ResetGroupResultCommand request, CancellationToken cancellationToken)
    {
        var existing = await _groupResultRepo.GetByGroupAsync(request.Group, cancellationToken);
        if (existing == null) return true;

        var predictions = await _groupRankRepo.GetByGroupAsync(request.Group, cancellationToken);
        foreach (var pred in predictions)
        {
            pred.Points = 0;
            _groupRankRepo.Update(pred);
        }

        _groupResultRepo.Remove(existing);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(CalculateGroupRankScoresCommand request, CancellationToken cancellationToken)
    {
        var allResults = await _groupResultRepo.GetAllAsync(cancellationToken);
        foreach (var result in allResults)
        {
            var predictions = await _groupRankRepo.GetByGroupAsync(result.Group, cancellationToken);
            foreach (var pred in predictions)
            {
                pred.Points = _scoringService.CalculateGroupRankScore(
                    pred.FirstTeam, pred.SecondTeam, pred.ThirdTeam, pred.FourthTeam,
                    result.FirstTeam, result.SecondTeam, result.ThirdTeam, result.FourthTeam);
                _groupRankRepo.Update(pred);
            }
        }
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(SetTournamentPhaseCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepo.GetActiveTournamentAsync(cancellationToken);
        if (tournament == null) throw new Exception("No active tournament found.");
        if (!Enum.TryParse<TournamentPhase>(request.Phase, out var phase))
            throw new Exception("Invalid phase.");
        tournament.CurrentPhase = phase;
        _tournamentRepo.Update(tournament);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(SetPrizePoolCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepo.GetActiveTournamentAsync(cancellationToken);
        if (tournament == null) throw new Exception("No active tournament found.");
        tournament.PrizePool = request.Amount;
        _tournamentRepo.Update(tournament);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(LockAllPredictionsCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepo.GetActiveTournamentAsync(cancellationToken);
        if (tournament == null) throw new Exception("No active tournament found.");
        tournament.ArePredictionsLocked = request.IsLocked;
        _tournamentRepo.Update(tournament);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByHandleAsync(request.Handle, cancellationToken);
        if (user == null) throw new Exception("User not found.");

        var tournament = await _tournamentRepo.GetActiveTournamentAsync(cancellationToken);
        if (tournament == null) throw new Exception("No active tournament found.");

        if (request.Amount <= 0)
        {
            if (user.IsPaid)
            {
                tournament.PrizePool -= user.PaidAmount;
                user.IsPaid = false;
                user.PaidAmount = 0;
            }
        }
        else
        {
            if (user.IsPaid)
            {
                tournament.PrizePool = tournament.PrizePool - user.PaidAmount + request.Amount;
            }
            else
            {
                tournament.PrizePool += request.Amount;
            }
            
            user.IsPaid = true;
            user.PaidAmount = request.Amount;
        }

        _userRepo.Update(user);
        _tournamentRepo.Update(tournament);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(ToggleUserPredictionUnlockCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) throw new Exception("User not found.");
        user.IsPredictionUnlocked = request.IsUnlocked;
        _userRepo.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<GroupResultSummaryDto>> Handle(GetGroupResultsQuery request, CancellationToken cancellationToken)
    {
        var results = await _groupResultRepo.GetAllAsync(cancellationToken);
        return results.Select(r => new GroupResultSummaryDto(r.Group, r.FirstTeam, r.SecondTeam, r.ThirdTeam, r.FourthTeam));
    }
}
