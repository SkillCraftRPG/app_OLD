using MediatR;
using SkillCraft.Contracts.Search;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Worlds.Queries;

internal class SearchWorldsQueryHandler : IRequestHandler<SearchWorldsQuery, SearchResults<World>>
{
  private readonly IWorldQuerier _worldQuerier;

  public SearchWorldsQueryHandler(IWorldQuerier worldQuerier)
  {
    _worldQuerier = worldQuerier;
  }

  public async Task<SearchResults<World>> Handle(SearchWorldsQuery query, CancellationToken cancellationToken)
  {
    return await _worldQuerier.SearchAsync(query.UserId, query.Payload, cancellationToken);
  }
}
