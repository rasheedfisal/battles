using Domain.Core.Primitives;

namespace Domain.Core.Errors;

public static class DomainErrors
{
    public static class Name
    {
        public static Error NullOrEmpty => new("Name.NullOrEmpty", "The name is required.");

        public static Error LongerThanAllowed => new("Name.LongerThanAllowed", "The name is longer than allowed.");
    }
    public static class General
    {
        public static Error UnProcessableRequest => new(
            "General.UnProcessableRequest",
            "The server could not process the request.");

        public static Error NotFound => new(
            "General.NotFound",
            "The requested entity was not found.");

        public static Error ServerError => new("General.ServerError", "The server encountered an unrecoverable error.");
    }
    public static class Battle
    {
        public static Error DuplicateName => new("Battle.DuplicateName", "The specified name is already in use.");
        public static Error UnableToAddBattle => new("Battle.UnableToAddBattle", "Cannot add battle.");
        public static Error UnableToUpdateBattle => new("Battle.UnableToUpdateBattle", "Cannot update battle.");
        public static Error UnableToDeleteBattle => new("Battle.UnableToDeleteBattle", "Cannot delete battle.");
    }
    public static class BattleDetails
    {
        public static Error BattleAlreadyStarted => new("BattleDetails.BattleAlreadyStarted", "The specified battle already startred.");
        public static Error BattleNotStarted => new("BattleDetails.BattleNotStarted", "The specified battle did not start.");
        public static Error BattleAlreadyEnded => new("BattleDetails.BattleAlreadyEnded", "The specified battle already Ended.");
        public static Error UnableToStartBattle => new("BattleDetails.UnableToStartBattle", "Cannot start battle.");
        public static Error UnableToEndBattle => new("BattleDetails.UnableToEndBattle", "Cannot end battle.");
    
    }

    public static class Samurai
    {
        public static Error DuplicateName => new("Samurai.DuplicateName", "The specified name is already in use.");
        public static Error UnableToAddSamurai => new("Samurai.UnableToAddSamurai", "Cannot add samurai.");
        public static Error UnableToUpdateSamurai => new("Samurai.UnableToUpdateSamurai", "Cannot update samurai.");
        public static Error UnableToDeleteSamurai => new("Samurai.UnableToDeleteSamurai", "Cannot delete samurai.");
    }

    public static class Horse
    {
        public static Error DuplicateName => new("Horse.DuplicateName", "The specified name is already in use.");
        public static Error UnableToAddHorse => new("Horse.UnableToAddHorse", "Cannot add horse.");
        public static Error UnableToUpdateHorse => new("Horse.UnableToUpdateHorse", "Cannot update horse.");
        public static Error UnableToDeleteHorse => new("Horse.UnableToDeleteHorse", "Cannot delete horse.");
    }
}