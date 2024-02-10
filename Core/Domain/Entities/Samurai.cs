using Domain.Core.Abstractions;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Core.ValueObjects;

namespace Domain.Entities;

public class Samurai: Entity, IAuditableEntity, ISoftDeletableEntity
{
    private Samurai(Name name, DateTime createdAt)
        : base(Guid.NewGuid())
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(createdAt, "The createdAt is required.", nameof(createdAt));

        Name = name;
        CreatedOnUtc = createdAt;
    }

    private Samurai(){}
    public Name Name { get; private set; } = Name.Empty;

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public static Samurai Create(Name name, DateTime createdAt)
    {
        var samurai = new Samurai(name, createdAt);

        return samurai;
    }
    public void ChangeName(Name name)
    {
        Ensure.NotEmpty(name, "name is required.", nameof(name));
        Name = name;
    }

    public void ChangeModifiedOnUtc(DateTime modifiedAt)
    {
        Ensure.NotEmpty(modifiedAt, "modifiedAt is required.", nameof(modifiedAt));
        ModifiedOnUtc = modifiedAt;
    }
    public void ChangeDeletedOnUtc(DateTime deletedAt)
    {
        Ensure.NotEmpty(deletedAt, "deletedAt is required.", nameof(deletedAt));
        DeletedOnUtc = deletedAt;
    }
}
