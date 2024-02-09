namespace Domain.Entities;

public interface IBattleRepository {
    Task<Guid> InsertAsync(Battle battle);

    Task<Battle?> GetByIdAsync(Guid id);

    Task UpdateAsync(Battle battle);
}