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
    // TODO(fpion): permissions

    return await _worldQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
