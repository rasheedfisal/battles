namespace Application.Queries;

public interface IGetBattleByIdQueryHandler
{
    Task<BattleResponse?> Handle(Guid id);
}
