using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Domain.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using MediatR;
using BolaoCopaApp.Domain.Enums;

namespace BolaoCopaApp.Application.Handlers;

public class AdminHandlers :
    IRequestHandler<RegisterMatchResultCommand, bool>,
    IRequestHandler<ToggleUserPaymentCommand, bool>
{
    private readonly IMatchRepository _matchRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _uow;

    public AdminHandlers(IMatchRepository matchRepo, IUserRepository userRepo, IUnitOfWork uow)
    {
        _matchRepo = matchRepo;
        _userRepo = userRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(RegisterMatchResultCommand request, CancellationToken cancellationToken)
    {
        var match = await _matchRepo.GetByIdAsync(Guid.Parse(request.MatchId), cancellationToken);
        if (match == null) throw new Exception("Match not found");

        match.HomeScore = request.HomeScore;
        match.AwayScore = request.AwayScore;
        match.Status = MatchStatus.Locked; // Locking the match since result is in

        _matchRepo.Update(match);
        await _uow.SaveChangesAsync(cancellationToken);

        // Optionally, fire MediatR event to trigger recalculation
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
}
