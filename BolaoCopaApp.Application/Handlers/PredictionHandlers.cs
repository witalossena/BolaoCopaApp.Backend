using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using BolaoCopaApp.Domain.Services;
using MediatR;

namespace BolaoCopaApp.Application.Handlers;

public class PredictionHandlers :
    IRequestHandler<SubmitPredictionCommand, bool>,
    IRequestHandler<SubmitGroupRankCommand, bool>,
    IRequestHandler<SubmitKnockoutPredictionCommand, bool>,
    IRequestHandler<ClearKnockoutPredictionsCommand, bool>,
    IRequestHandler<ClearAllPredictionsCommand, bool>,
    IRequestHandler<SubmitSpecialPredictionCommand, bool>
{
    private readonly IPredictionRepository _predictionRepo;
    private readonly IMatchRepository _matchRepo;
    private readonly IGroupRankPredictionRepository _groupRankRepo;
    private readonly IKnockoutPredictionRepository _knockoutRepo;
    private readonly ISpecialPredictionRepository _specialRepo;
    private readonly ITournamentRepository _tournamentRepo;
    private readonly PredictionValidationService _validationService;
    private readonly IUnitOfWork _uow;

    public PredictionHandlers(
        IPredictionRepository predictionRepo,
        IMatchRepository matchRepo,
        IGroupRankPredictionRepository groupRankRepo,
        IKnockoutPredictionRepository knockoutRepo,
        ISpecialPredictionRepository specialRepo,
        ITournamentRepository tournamentRepo,
        PredictionValidationService validationService,
        IUnitOfWork uow)
    {
        _predictionRepo = predictionRepo;
        _matchRepo = matchRepo;
        _groupRankRepo = groupRankRepo;
        _knockoutRepo = knockoutRepo;
        _specialRepo = specialRepo;
        _tournamentRepo = tournamentRepo;
        _validationService = validationService;
        _uow = uow;
    }

    private async Task EnsureNotLocked(CancellationToken ct)
    {
        var tournament = await _tournamentRepo.GetActiveTournamentAsync(ct);
        if (tournament != null && tournament.ArePredictionsLocked)
            throw new Exception("All predictions are currently locked by admin.");
    }

    public async Task<bool> Handle(SubmitPredictionCommand request, CancellationToken cancellationToken)
    {
        await EnsureNotLocked(cancellationToken);
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");

        if (!_validationService.IsMatchPredictionAllowed(match))
            throw new Exception("Prediction not allowed for this match.");

        var existing = await _predictionRepo.GetByUserAndMatchAsync(request.UserId, match.Id, cancellationToken);
        if (existing != null)
        {
            existing.HomeScore = request.HomeScore;
            existing.AwayScore = request.AwayScore;
            existing.UpdatedAt = DateTime.UtcNow;
            _predictionRepo.Update(existing);
        }
        else
        {
            var prediction = new Prediction
            {
                UserId = request.UserId,
                MatchId = match.Id,
                HomeScore = request.HomeScore,
                AwayScore = request.AwayScore
            };
            await _predictionRepo.AddAsync(prediction, cancellationToken);
        }

        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(SubmitGroupRankCommand request, CancellationToken cancellationToken)
    {
        await EnsureNotLocked(cancellationToken);
        var existing = await _groupRankRepo.GetByUserAndGroupAsync(request.UserId, request.Group, cancellationToken);
        if (existing != null)
        {
            existing.FirstTeam = request.FirstTeam;
            existing.SecondTeam = request.SecondTeam;
            existing.ThirdTeam = request.ThirdTeam;
            existing.FourthTeam = request.FourthTeam;
            _groupRankRepo.Update(existing);
        }
        else
        {
            var prediction = new GroupRankPrediction
            {
                UserId = request.UserId,
                Group = request.Group,
                FirstTeam = request.FirstTeam,
                SecondTeam = request.SecondTeam,
                ThirdTeam = request.ThirdTeam,
                FourthTeam = request.FourthTeam
            };
            await _groupRankRepo.AddAsync(prediction, cancellationToken);
        }
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(SubmitKnockoutPredictionCommand request, CancellationToken cancellationToken)
    {
        await EnsureNotLocked(cancellationToken);
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");

        var existing = await _knockoutRepo.GetByUserAndMatchAsync(request.UserId, match.Id, cancellationToken);
        if (existing != null)
        {
            existing.WinnerTeam = request.WinnerTeam;
            existing.HomeScore = request.HomeScore;
            existing.AwayScore = request.AwayScore;
            _knockoutRepo.Update(existing);
        }
        else
        {
            var prediction = new KnockoutPrediction
            {
                UserId = request.UserId,
                MatchId = match.Id,
                WinnerTeam = request.WinnerTeam,
                HomeScore = request.HomeScore,
                AwayScore = request.AwayScore
            };
            await _knockoutRepo.AddAsync(prediction, cancellationToken);
        }
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(SubmitSpecialPredictionCommand request, CancellationToken cancellationToken)
    {
        await EnsureNotLocked(cancellationToken);
        var existing = await _specialRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existing != null)
        {
            existing.Champion = request.Prediction.Champion;
            existing.RunnerUp = request.Prediction.RunnerUp;
            existing.ThirdPlace = request.Prediction.ThirdPlace;
            existing.OtherFinalist = request.Prediction.OtherFinalist;
            existing.TopScorer = request.Prediction.TopScorer;
            existing.MostAssists = request.Prediction.MostAssists;
            existing.MVP = request.Prediction.MVP;
            existing.GoldenBoy = request.Prediction.GoldenBoy;
            _specialRepo.Update(existing);
        }
        else
        {
            var prediction = new SpecialPrediction
            {
                UserId = request.UserId,
                Champion = request.Prediction.Champion,
                RunnerUp = request.Prediction.RunnerUp,
                ThirdPlace = request.Prediction.ThirdPlace,
                OtherFinalist = request.Prediction.OtherFinalist,
                TopScorer = request.Prediction.TopScorer,
                MostAssists = request.Prediction.MostAssists,
                MVP = request.Prediction.MVP,
                GoldenBoy = request.Prediction.GoldenBoy,
            };
            await _specialRepo.AddAsync(prediction, cancellationToken);
        }
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(ClearKnockoutPredictionsCommand request, CancellationToken cancellationToken)
    {
        await EnsureNotLocked(cancellationToken);
        var existing = await _knockoutRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        _knockoutRepo.RemoveRange(existing);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(ClearAllPredictionsCommand request, CancellationToken cancellationToken)
    {
        await EnsureNotLocked(cancellationToken);
        var matchPreds = await _predictionRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        _predictionRepo.RemoveRange(matchPreds);

        var groupPreds = await _groupRankRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        _groupRankRepo.RemoveRange(groupPreds);

        var knockoutPreds = await _knockoutRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        _knockoutRepo.RemoveRange(knockoutPreds);

        var special = await _specialRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        if (special != null) _specialRepo.Remove(special);

        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
