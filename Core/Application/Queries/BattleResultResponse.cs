using Domain.Entities;

namespace Application.Queries;

public class BattleResultResponse
{
    public Battle battle { get; set; }
    public List<SamuraiHorseResponse> SamuraiHorses { get; set; }
}
