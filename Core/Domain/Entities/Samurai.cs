using Domain.Core.Abstractions;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Core.ValueObjects;

namespace Domain.Entities;

public class Samurai: Entity, IAuditableEntity, ISoftDeletableEntity
{
    private Samurai(Name name)
        : base(Guid.NewGuid())
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));

        Name = name;
    }

    private Samurai(){}
    public Name Name { get; private set; } = Name.Empty;

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public DateTime? DeletedOnUtc { get; }

    public static Samurai Create(Name name)
    {
        var samurai = new Samurai(name);

        return samurai;
    }
    public void ChangeName(Name name)
    {
        Ensure.NotEmpty(name, "name is required.", nameof(name));
        Name = name;
    }
}
