namespace Domain.Core.Primitives;

public sealed class Error : ValueObject
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
     public string Code { get; }
     public string Message { get; }

    
    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
        yield return Message;
    }

    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullError = new("Error.NullValue", "the specified result value is null.");
    
}
