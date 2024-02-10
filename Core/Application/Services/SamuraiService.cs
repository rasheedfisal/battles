using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Core.ValueObjects;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public sealed class SamuraiService
{
    private readonly ISamuraiRepository _samuraiRepository;

    public SamuraiService(ISamuraiRepository samuraiRepository)
    {
        _samuraiRepository = samuraiRepository;
    }

    public async Task<Result<Samurai>> Create(string name, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(name);
        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult);

        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Samurai>(firstFailureOrSuccess.Error);
        }

         bool isNameUnique = await _samuraiRepository.AnyAsync(x => x.Name.Value == nameResult.Value, cancellationToken);
        if (!isNameUnique)
        {
            return Result.Failure<Samurai>(DomainErrors.Samurai.DuplicateName);
        }

       var samurai = Samurai.Create(nameResult.Value, DateTime.UtcNow);

        var createdSamurai = await _samuraiRepository.InsertAsync(samurai);
        await _samuraiRepository.CompleteAsync(cancellationToken);

        if (createdSamurai is null){
            return Result.Failure<Samurai>(DomainErrors.Samurai.UnableToAddSamurai);
        } 
        

        return Result.Success(createdSamurai);
    }
    public async Task<Result<Samurai>> Update(Guid id, string name, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(name);
        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult);


        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Samurai>(firstFailureOrSuccess.Error);
        }

        var samurai = await _samuraiRepository.FindOneAsync(x => x.Id == id, cancellationToken);

        if (samurai is null)
        {
            return Result.Failure<Samurai>(DomainErrors.General.NotFound);
        }
        
        samurai.ChangeName(nameResult.Value);
        samurai.ChangeModifiedOnUtc(DateTime.UtcNow);

        var updatedSamurai = await _samuraiRepository.UpdateAsync(samurai, id);
        await _samuraiRepository.CompleteAsync(cancellationToken);

         if (updatedSamurai is null){
            return Result.Failure<Samurai>(DomainErrors.Samurai.UnableToUpdateSamurai);
        } 

       return Result.Success(updatedSamurai);
    }

    public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _samuraiRepository.DeleteAsync(id);
        await _samuraiRepository.CompleteAsync(cancellationToken);

        if (!result)
        {
            return Result.Failure<bool>(DomainErrors.General.UnProcessableRequest);
        }

        return Result.Success(true);
    }

    public async Task<Result<Samurai>> Get(Guid id, CancellationToken cancellationToken){
        var samurai = await _samuraiRepository.FindOneAsync(x => x.Id == id, cancellationToken);

        if (samurai is null)
        {
            return Result.Failure<Samurai>(DomainErrors.General.NotFound);
        }
        
        return Result.Success(samurai);
    }

    public async Task<Result<IEnumerable<Samurai>>> GetAll(CancellationToken cancellationToken){
        var samurais = await _samuraiRepository.GetAllAsync(cancellationToken);

        if (samurais is null)
        {
            return Result.Failure<IEnumerable<Samurai>>(DomainErrors.General.NotFound);
        }
        
        return Result.Success(samurais);
    }
}
