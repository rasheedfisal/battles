using Domain.Core.Abstractions;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Core.ValueObjects;

namespace Domain.Entities;

public class Battle: Entity, IAuditableEntity, ISoftDeletableEntity
{
    private Battle(Name name)
        : base(Guid.NewGuid())
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));

        Name = name;
        
    }
    private Battle(){}
    public Name Name { get; private set; } = Name.Empty;

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public DateTime? DeletedOnUtc { get; }


    public static Battle Create(Name name)
    {
        var battle = new Battle(name);

        return battle;
    }

    public void ChangeName(Name name)
    {
        Ensure.NotEmpty(name, "name is required.", nameof(name));
        Name = name;
    }
}