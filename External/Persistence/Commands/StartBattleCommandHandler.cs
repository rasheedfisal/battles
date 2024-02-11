using Application.Commands;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Commands;

public class StartBattleCommandHandler : IStartBattleCommandHandler
{
    private readonly ApplicationDbContext _context;

    public StartBattleCommandHandler(ApplicationDbContext context)
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

        if (battleDetails.Count > 0)
        {
            return Result.Failure<bool>(DomainErrors.BattleDetails.BattleAlreadyStarted);
        }

        var getItems = await GenerateBattleItems(battleId, cancellationToken);

        if (getItems.IsFailure)
        {
            return Result.Failure<bool>(DomainErrors.BattleDetails.UnableToStartBattle);
        }

        _context.BattleDetails.BulkInsertOptimized(getItems.Value);

        // await _context.BulkSaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }

    private async Task<Result<List<BattleDetail>>> GenerateBattleItems(Guid battleId, CancellationToken cancellationToken) 
    {

       var battleDetails = new List<BattleDetail>();
        var resultSamurais = await _context
                        .Samurais
                        .Where(x => x.DeletedOnUtc == null)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
        
        if (resultSamurais.Count == 0)
        {
            Result.Failure<BattleDetail>(DomainErrors.General.NotFound);
        }
        var allSamurais = resultSamurais;

        var resultHorses = await _context
                            .Horses
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);
        if (resultHorses.Count == 0)
        {
            Result.Failure<BattleDetail>(DomainErrors.General.NotFound);
        }

        var allHorses = resultHorses;

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

        if (battleDetails.Count == 0)
        {
            Result.Failure<BattleDetail>(DomainErrors.BattleDetails.AllHorsesFoughtWithGivenSamurais);
        }
        
        return Result.Success(battleDetails);
    }
}