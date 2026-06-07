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
    IRequestHandler<ClearKnockoutPredictionsCommand, bool>
{
    private readonly IPredictionRepository _predictionRepo;
    private readonly IMatchRepository _matchRepo;
    private readonly IGroupRankPredictionRepository _groupRankRepo;
    private readonly IKnockoutPredictionRepository _knockoutRepo;
    private readonly PredictionValidationService _validationService;
    private readonly IUnitOfWork _uow;

    public PredictionHandlers(
        IPredictionRepository predictionRepo, 
        IMatchRepository matchRepo,
        IGroupRankPredictionRepository groupRankRepo,
        IKnockoutPredictionRepository knockoutRepo,
        PredictionValidationService validationService,
        IUnitOfWork uow)
    {
        _predictionRepo = predictionRepo;
        _matchRepo = matchRepo;
        _groupRankRepo = groupRankRepo;
        _knockoutRepo = knockoutRepo;
        _validationService = validationService;
        _uow = uow;
    }

    public async Task<bool> Handle(SubmitPredictionCommand request, CancellationToken cancellationToken)
    {
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
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");

        var existing = await _knockoutRepo.GetByUserAndMatchAsync(request.UserId, match.Id, cancellationToken);
        if (existing != null)
        {
            existing.WinnerTeam = request.WinnerTeam;
            _knockoutRepo.Update(existing);
        }
        else
        {
            var prediction = new KnockoutPrediction
            {
                UserId = request.UserId,
                MatchId = match.Id,
                WinnerTeam = request.WinnerTeam
            };
            await _knockoutRepo.AddAsync(prediction, cancellationToken);
        }
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(ClearKnockoutPredictionsCommand request, CancellationToken cancellationToken)
    {
        var existing = await _knockoutRepo.GetByUserIdAsync(request.UserId, cancellationToken);
        _knockoutRepo.RemoveRange(existing);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
