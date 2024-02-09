using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class SamuraiService
{
    private readonly ISamuraiRepository _samuraiRepository;

    public SamuraiService(ISamuraiRepository samuraiRepository)
    {
        _samuraiRepository = samuraiRepository;
    }

    public async Task<Guid> Create(string name)
    {
        var samurai = new Samurai{
            Id = Guid.NewGuid(),
            Name = name,
        };

        var createdSamurai = await _samuraiRepository.InsertAsync(samurai);
        await _samuraiRepository.CompleteAsync();

        if (createdSamurai is null){
            throw new ApplicationException("cannot create samurai");
        } 
        

        return createdSamurai.Id;
    }
    public async Task Update(Guid id){
        var samurai = await _samuraiRepository.FindOneAsync(x => x.Id == id);

        if (samurai is null)
        {
            throw new ApplicationException("samurai not found");
        }
        
        await _samuraiRepository.UpdateAsync(samurai, id);
        await _samuraiRepository.CompleteAsync();
    }

    public async Task Delete(Guid id){
        var samurai = await _samuraiRepository.FindOneAsync(x => x.Id == id);

        if (samurai is null)
        {
            throw new ApplicationException("samurai not found");
        }
        
        await _samuraiRepository.DeleteAsync(id);
        await _samuraiRepository.CompleteAsync();
    }

    public async Task<Samurai> Get(Guid id){
        var samurai = await _samuraiRepository.FindOneAsync(x => x.Id == id);

        if (samurai is null)
        {
            throw new ApplicationException("samurai not found");
        }
        
        return samurai;
    }

    public async Task<IEnumerable<Samurai>> GetAll(){
        var samurais = await _samuraiRepository.GetAllAsync();

        if (samurais is null)
        {
            throw new ApplicationException("samurais cannot be retrived");
        }
        
        return samurais;
    }
}
