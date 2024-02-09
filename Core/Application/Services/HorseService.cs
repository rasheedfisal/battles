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

    public async Task<Guid> Create(string name)
    {
        var samurai = new Horse{
            Id = Guid.NewGuid(),
            Name = name,
        };

        var createdHorse = await _horseRepository.InsertAsync(samurai);
        await _horseRepository.CompleteAsync();

        if (createdHorse is null){
            throw new ApplicationException("cannot create samurai");
        } 
        

        return createdHorse.Id;
    }
    public async Task Update(Guid id){
        var samurai = await _horseRepository.FindOneAsync(x => x.Id == id);

        if (samurai is null)
        {
            throw new ApplicationException("samurai not found");
        }
        
        await _horseRepository.UpdateAsync(samurai, id);
        await _horseRepository.CompleteAsync();
    }

    public async Task Delete(Guid id){
        var samurai = await _horseRepository.FindOneAsync(x => x.Id == id);

        if (samurai is null)
        {
            throw new ApplicationException("samurai not found");
        }
        
        await _horseRepository.DeleteAsync(id);
        await _horseRepository.CompleteAsync();
    }

    public async Task<Horse> Get(Guid id){
        var samurai = await _horseRepository.FindOneAsync(x => x.Id == id);

        if (samurai is null)
        {
            throw new ApplicationException("samurai not found");
        }
        
        return samurai;
    }

    public async Task<IEnumerable<Horse>> GetAll(){
        var samurais = await _horseRepository.GetAllAsync();

        if (samurais is null)
        {
            throw new ApplicationException("samurais cannot be retrived");
        }
        
        return samurais;
    }
}
