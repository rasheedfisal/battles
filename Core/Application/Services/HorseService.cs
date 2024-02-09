using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class HorseService
{
    private readonly IHorseRepository _horseRepository;

    public HorseService(IHorseRepository horseRepository)
    {
        _horseRepository = horseRepository;
    }

    public async Task<Horse> Create(string name)
    {
        var horse = new Horse{
            Id = Guid.NewGuid(),
            Name = name,
        };

        var createdHorse = await _horseRepository.InsertAsync(horse);
        await _horseRepository.CompleteAsync();

        if (createdHorse is null){
            throw new ApplicationException("cannot create horse");
        } 
        

        return createdHorse;
    }
    public async Task<Horse> Update(Guid id, string name){
        var horse = await _horseRepository.FindOneAsync(x => x.Id == id);

        if (horse is null)
        {
            throw new ApplicationException("horse not found");
        }

        horse.Name = name;
        horse.UpdatedAt = DateTime.UtcNow;
        
        var updatedHorse = await _horseRepository.UpdateAsync(horse, id);
        await _horseRepository.CompleteAsync();

         if (updatedHorse is null){
            throw new ApplicationException("cannot update horse");
        } 

        return updatedHorse;
    }

    public async Task Delete(Guid id){
        var horse = await _horseRepository.FindOneAsync(x => x.Id == id);

        if (horse is null)
        {
            throw new ApplicationException("horse not found");
        }
        
        await _horseRepository.DeleteAsync(id);
        await _horseRepository.CompleteAsync();
    }

    public async Task<Horse> Get(Guid id){
        var horse = await _horseRepository.FindOneAsync(x => x.Id == id);

        if (horse is null)
        {
            throw new ApplicationException("horse not found");
        }
        
        return horse;
    }

    public async Task<IEnumerable<Horse>> GetAll(){
        var horses = await _horseRepository.GetAllAsync();

        if (horses is null)
        {
            throw new ApplicationException("horses cannot be retrived");
        }
        
        return horses;
    }
}
