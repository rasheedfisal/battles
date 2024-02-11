using Application.Commands;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Commands;

public class EndBattleCommandHandler : IEndBattleCommandHandler
{
    private readonly ApplicationDbContext _context;

    public EndBattleCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(Guid battleId, CancellationToken cancellationToken = default)
    {
        var battle = await _context
                        .Battles
                        .AsNoTracking()
                        .Where(x => x.Id == battleId)
                        .FirstOrDefaultAsync(cancellationToken);
        
        if (battle is null)
        {
            return Result.Failure<bool>(DomainErrors.General.NotFound);
        }

        var battleDetails = await _context
                                .BattleDetails
                                .AsNoTracking()
                                .Where(x => x.BattleId == battleId)
                                .ToListAsync(cancellationToken);

        if (battleDetails.Count == 0)
        {
            return Result.Failure<bool>(DomainErrors.BattleDetails.BattleNotStarted);
        }

        var hasAlreadyEnded = battleDetails.Any(x => x.HorseRideEndDate != null);

        if (hasAlreadyEnded)
        {
            return Result.Failure<bool>(DomainErrors.BattleDetails.BattleAlreadyEnded);
        }

        await _context.BattleDetails.ExecuteUpdateAsync(
            s => s.SetProperty(
                p => p.HorseRideEndDate,
                p => DateTime.UtcNow
            ),
            cancellationToken
        );

        // await _context.BulkSaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}