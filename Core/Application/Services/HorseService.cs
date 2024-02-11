using Application.Core.Data;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Core.ValueObjects;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public sealed class HorseService
{
    private readonly IHorseRepository _horseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HorseService(IHorseRepository horseRepository, IUnitOfWork unitOfWork)
    {
        _horseRepository = horseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Horse>> Create(string name, CancellationToken cancellationToken)
    {
        Result<Name> nameResult = Name.Create(name);
        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(nameResult);

         if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Horse>(firstFailureOrSuccess.Error);
        }
        var newValue = nameResult.Value;
        var isNameUnique = !await _horseRepository.AnyAsync(x => x.Name == newValue, cancellationToken);
        if (!isNameUnique)
        {
            return Result.Failure<Horse>(DomainErrors.Horse.DuplicateName);
        }
         var horse = Horse.Create(newValue);

        var createdHorse = await _horseRepository.InsertAsync(horse);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
        
        var updatedHorse = await _horseRepository.UpdateAsync(horse, id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

         if (updatedHorse is null){
            return Result.Failure<Horse>(DomainErrors.Horse.UnableToUpdateHorse);
        } 

         return Result.Success(updatedHorse);
    }

    public async Task<Result<bool>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _horseRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
