using Domain.Entities;

namespace Application.Queries;

public class SamuraiHorseResponse
{
    public Samurai Samurai { get; set; }
    public Horse Horse { get; set; }

    public DateTime HorseRideStartDate { get; set; }
    public DateTime HorseRideEndDate { get; set; }

}
