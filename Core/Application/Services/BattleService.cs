using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Core.ValueObjects;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public sealed class BattleService
{
    private readonly IBattleRepository _battleRepository;

    public BattleService(IBattleRepository battleRepository)
    {
        _battleRepository = battleRepository;
    }

    public async Task<Result<Battle>> Create(string name, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(name);
        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult);

        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Battle>(firstFailureOrSuccess.Error);
        }
        bool isNameUnique = await _battleRepository.AnyAsync(x => x.Name.Value == nameResult.Value, cancellationToken);
        if (!isNameUnique)
        {
            return Result.Failure<Battle>(DomainErrors.Battle.DuplicateName);
        }

        var battle = Battle.Create(nameResult.Value, DateTime.UtcNow);

        var createdBattle = await _battleRepository.InsertAsync(battle);
        await _battleRepository.CompleteAsync(cancellationToken);

        if (createdBattle is null){
            return Result.Failure<Battle>(DomainErrors.Battle.UnableToAddBattle);
        } 

        return Result.Success(createdBattle);
    }
    public async Task<Result<Battle>> Update(Guid id, string name, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(name);
        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult);


        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Battle>(firstFailureOrSuccess.Error);
        }

        var battle = await _battleRepository.FindOneAsync(x => x.Id == id, cancellationToken);

        if (battle is null)
        {
           return Result.Failure<Battle>(DomainErrors.General.NotFound);
        }

        battle.ChangeName(nameResult.Value);
        battle.ChangeModifiedOnUtc(DateTime.UtcNow);
        
        var updatedBattle = await _battleRepository.UpdateAsync(battle, id);
        await _battleRepository.CompleteAsync(cancellationToken);

        if (updatedBattle is null){
            return Result.Failure<Battle>(DomainErrors.Battle.UnableToUpdateBattle);
        } 

        return Result.Success(updatedBattle);
    }

    public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken){
        var result = await _battleRepository.DeleteAsync(id);
        await _battleRepository.CompleteAsync(cancellationToken);
        if (!result)
        {
            return Result.Failure<bool>(DomainErrors.General.UnProcessableRequest);
        }
        return Result.Success(true);
    }

    public async Task<Result<Battle>> Get(Guid id, CancellationToken cancellationToken){
        var battle = await _battleRepository.FindOneAsync(x => x.Id == id, cancellationToken);

        if (battle is null)
        {
            return Result.Failure<Battle>(DomainErrors.General.NotFound);
        }
        
        return Result.Success(battle);
    }

    public async Task<Result<IEnumerable<Battle>>> GetAll(CancellationToken cancellationToken){
        var battles = await _battleRepository.GetAllAsync(cancellationToken);

        if (battles is null)
        {
            return Result.Failure<IEnumerable<Battle>>(DomainErrors.General.NotFound);
        }
        
        return Result.Success(battles);
    }
}
