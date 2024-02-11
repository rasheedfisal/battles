using Domain.Core.Primitives.Result;

namespace Application.Queries;

public interface IGetAllBattleResultsQueryHandler
{
   Task<Result<List<BattleResultResponse>>> Handle(CancellationToken cancellationToken = default);
}
