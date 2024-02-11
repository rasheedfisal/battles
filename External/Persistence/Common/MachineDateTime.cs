using Application.Core.Common;

namespace Persistence.Common;

public sealed class MachineDateTime : IDateTime
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}