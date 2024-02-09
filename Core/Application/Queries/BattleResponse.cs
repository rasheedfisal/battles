namespace Application.Queries;

public class BattleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public DateTime BattleStartDate { get; set; }
    public DateTime BattleEndDate { get; set; }
}
