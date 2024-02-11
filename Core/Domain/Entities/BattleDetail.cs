using Domain.Core.Abstractions;
using Domain.Core.Primitives;
using Domain.Core.Utility;

namespace Domain.Entities;

public class BattleDetail : Entity
{
    private BattleDetail(Guid battleId, Guid samuraiId, Guid horseId, DateTime horseRideStartDate) 
        :base(Guid.NewGuid())
    {
        Ensure.NotEmpty(battleId, "The battleId is required.", nameof(battleId));
        Ensure.NotEmpty(samuraiId, "The samuraiId is required.", nameof(samuraiId));
        Ensure.NotEmpty(horseId, "The horseId is required.", nameof(horseId));
        Ensure.NotEmpty(horseRideStartDate, "The horseRideStartDate is required.", nameof(horseRideStartDate));

        BattleId = battleId;
        SamuraiId = samuraiId;
        HorseId = horseId;
        HorseRideStartDate = horseRideStartDate;


    }
    private BattleDetail(){}

    public Guid BattleId { get; private set; }
    public Battle? Battle { get; }
    public Guid SamuraiId { get; private set; }
    public Samurai? Samurai { get; }
    public Guid HorseId { get; private set; }
    public Horse? Horse { get; }

    public DateTime HorseRideStartDate { get; private set; }
    public DateTime? HorseRideEndDate { get; private set; }

    public static BattleDetail Create(Guid battleId, Guid samuraiId, Guid horseId, DateTime horseRideStartDate)
    {
        var battleDetail = new BattleDetail(battleId, samuraiId, horseId, horseRideStartDate);

        return battleDetail;
    }
}
