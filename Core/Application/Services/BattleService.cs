﻿using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class BattleService
{
    private readonly IBattleRepository _battleRepository;

    public BattleService(IBattleRepository battleRepository)
    {
        _battleRepository = battleRepository;
    }

    public async Task<Battle> Create(string name)
    {
        var battle = new Battle{
            Id = Guid.NewGuid(),
            Name = name,
        };

        var createdBattle = await _battleRepository.InsertAsync(battle);
        await _battleRepository.CompleteAsync();

        if (createdBattle is null){
            throw new ApplicationException("cannot create battle");
        } 

        return createdBattle;
    }
    public async Task<Battle> Update(Guid id, string name){
        var battle = await _battleRepository.FindOneAsync(x => x.Id == id);

        if (battle is null)
        {
            throw new ApplicationException("battle not found");
        }

        battle.Name = name;
        battle.UpdatedAt = DateTime.UtcNow;
        
        var updatedBattle = await _battleRepository.UpdateAsync(battle, id);
        await _battleRepository.CompleteAsync();

        if (updatedBattle is null){
            throw new ApplicationException("cannot update battle");
        } 

        return updatedBattle;
    }

    public async Task Delete(Guid id){
        var battle = await _battleRepository.FindOneAsync(x => x.Id == id);

        if (battle is null)
        {
            throw new ApplicationException("battle not found");
        }
        
        await _battleRepository.DeleteAsync(id);
        await _battleRepository.CompleteAsync();
    }

    public async Task<Battle> Get(Guid id){
        var battle = await _battleRepository.FindOneAsync(x => x.Id == id);

        if (battle is null)
        {
            throw new ApplicationException("battle not found");
        }
        
        return battle;
    }

    public async Task<IEnumerable<Battle>> GetAll(){
        var battles = await _battleRepository.GetAllAsync();

        if (battles is null)
        {
            throw new ApplicationException("battles cannot be retrived");
        }
        
        return battles;
    }
}
