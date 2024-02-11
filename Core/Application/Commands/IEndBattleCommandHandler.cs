using Domain.Core.Primitives.Result;

namespace Application.Commands;

public interface IEndBattleCommandHandler
{
    Task<Result<bool>> Handle(Guid battleId, CancellationToken cancellationToken = default);
}
