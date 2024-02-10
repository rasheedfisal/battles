using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;


namespace Domain.Core.ValueObjects;

public class Name: ValueObject
{
    public const int MaxLength = 200;
    private Name(string value) => Value = value;
    public string Value { get; }

    public static implicit operator string(Name name) => name.Value;

    public static Result<Name> Create(string name) =>
        Result.Create(name, DomainErrors.Name.NullOrEmpty)
            .Ensure(n => !string.IsNullOrWhiteSpace(n), DomainErrors.Name.NullOrEmpty)
            .Ensure(n => n.Length <= MaxLength, DomainErrors.Name.LongerThanAllowed)
            .Map(f => new Name(f));

    public static Name Empty => new("");

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}