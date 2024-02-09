using Domain.Entities;

namespace Application.Services;

public class BattleService
{
    private readonly IBattleRepository _battleRepository;

    public BattleService(IBattleRepository battleRepository)
    {
        _battleRepository = battleRepository;
    }

    public async Task<Guid> CreateAsync(string name){
        var battle = new Battle{
            Id = Guid.NewGuid(),
            Name = name,
            BattleStartDate = DateTime.UtcNow,
        };

        var battleId = await _battleRepository.InsertAsync(battle);

        return battleId;
    }
    public async Task UpdateAsync(Guid id){
        var battle = await _battleRepository.GetByIdAsync(id) ?? throw new ApplicationException("battle not found");
        
        await _battleRepository.UpdateAsync(battle);
    }
}
