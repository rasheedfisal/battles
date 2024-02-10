using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Core.ValueObjects;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public sealed class HorseService
{
    private readonly IHorseRepository _horseRepository;

    public HorseService(IHorseRepository horseRepository)
    {
        _horseRepository = horseRepository;
    }

    public async Task<Result<Horse>> Create(string name, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(name);
        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult);

         if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Horse>(firstFailureOrSuccess.Error);
        }
        bool isNameUnique = await _horseRepository.AnyAsync(x => x.Name.Value == nameResult.Value, cancellationToken);
        if (!isNameUnique)
        {
            return Result.Failure<Horse>(DomainErrors.Horse.DuplicateName);
        }
         var horse = Horse.Create(nameResult.Value, DateTime.UtcNow);

        var createdHorse = await _horseRepository.InsertAsync(horse);
        await _horseRepository.CompleteAsync(cancellationToken);

        if (createdHorse is null){
             return Result.Failure<Horse>(DomainErrors.Horse.UnableToAddHorse);
        } 
        

       return Result.Success(createdHorse);
    }
    
    public async Task<Result<Horse>> Update(Guid id, string name, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(name);
        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult);


        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Horse>(firstFailureOrSuccess.Error);
        }

        var horse = await _horseRepository.FindOneAsync(x => x.Id == id, cancellationToken);

        if (horse is null)
        {
            return Result.Failure<Horse>(DomainErrors.General.NotFound);
        }

        horse.ChangeName(nameResult.Value);
        horse.ChangeModifiedOnUtc(DateTime.UtcNow);
        
        var updatedHorse = await _horseRepository.UpdateAsync(horse, id);
        await _horseRepository.CompleteAsync(cancellationToken);

         if (updatedHorse is null){
            return Result.Failure<Horse>(DomainErrors.Horse.UnableToUpdateHorse);
        } 

         return Result.Success(updatedHorse);
    }

    public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _horseRepository.DeleteAsync(id);
        await _horseRepository.CompleteAsync(cancellationToken);

        if (!result)
        {
            return Result.Failure<bool>(DomainErrors.General.UnProcessableRequest);
        }
        return Result.Success(true);
    }

    public async Task<Result<Horse>> Get(Guid id, CancellationToken cancellationToken){
        var horse = await _horseRepository.FindOneAsync(x => x.Id == id, cancellationToken);

        if (horse is null)
        {
            return Result.Failure<Horse>(DomainErrors.General.NotFound);
        }
        
        return Result.Success(horse);
    }

    public async Task<Result<IEnumerable<Horse>>> GetAll(CancellationToken cancellationToken){
        var horses = await _horseRepository.GetAllAsync(cancellationToken);

        if (horses is null)
        {
            return Result.Failure<IEnumerable<Horse>>(DomainErrors.General.NotFound);
        }
        
        return Result.Success(horses);
    }
}
