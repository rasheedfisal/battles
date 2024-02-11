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

        var getItems = await GenerateBattleItems(battleId, cancellationToken);

        if (getItems.IsFailure)
        {
            return Result.Failure<bool>(DomainErrors.BattleDetails.UnableToStartBattle);
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

    private async Task<Result<List<BattleDetail>>> GenerateBattleItems(Guid battleId, CancellationToken cancellationToken) 
    {

       var battleDetails = new List<BattleDetail>();
        var allSamurais = await _context
                        .Samurais
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
        if (allSamurais.Count == 0)
        {
            Result.Failure<Lazy<BattleDetail>>(DomainErrors.General.NotFound);
        }

        var allHorses = await _context
                            .Horses
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);
        if (allHorses.Count == 0)
        {
            Result.Failure<Lazy<BattleDetail>>(DomainErrors.General.NotFound);
        }

        foreach (var samurai in allSamurais)
        {
            
            foreach (var horse in allHorses)
            {
                var isUniqueBattleSamuraiHorse = !await _context.BattleDetails
                                                .AsNoTracking()
                                                .AnyAsync(x => x.BattleId == battleId && x.SamuraiId == samurai.Id && x.HorseId == horse.Id, cancellationToken);

                if (isUniqueBattleSamuraiHorse)
                {
                    var newDetails = BattleDetail.Create(battleId, samurai.Id, horse.Id, DateTime.UtcNow);
                    battleDetails.Add(newDetails);
                    break;
                }else {
                    int tmp = allHorses.IndexOf(horse);
                    allHorses.Add(horse);
                    allHorses.RemoveAt(tmp);
                }
            }
            // try
            // {
            // }
            // catch (DbUpdateException ex)
            // {
            //     if (ex.InnerException?.InnerException is SqlException innerException && (innerException.Number == 2627 || innerException.Number == 2601))
            //     {
                    
            //     }
            //     else
            //     {
            //         Result.Failure<List<BattleDetail>>(DomainErrors.General.ServerError);
            //     }
            // }
        }
        
        return Result.Success(battleDetails);
    }
}