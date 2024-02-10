using Domain.Core.Abstractions;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Core.ValueObjects;

namespace Domain.Entities;

public class Horse: Entity, IAuditableEntity, ISoftDeletableEntity
{
    private Horse(Name name)
        : base(Guid.NewGuid())
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));

        Name = name;
    }
    private Horse(){}
    public Name Name { get; private set; } = Name.Empty;

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public DateTime? DeletedOnUtc { get; }

    public static Horse Create(Name name)
    {
        var horse = new Horse(name);

        return horse;
    }

    public void ChangeName(Name name)
    {
        Ensure.NotEmpty(name, "name is required.", nameof(name));
        Name = name;
    }

}
