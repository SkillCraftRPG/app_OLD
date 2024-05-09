using MediatR;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Worlds.Queries;

internal class ReadWorldQueryHandler : IRequestHandler<ReadWorldQuery, World?>
{
  private readonly IWorldQuerier _worldQuerier;

  public ReadWorldQueryHandler(IWorldQuerier worldQuerier)
  {
    _worldQuerier = worldQuerier;
  }

  public async Task<World?> Handle(ReadWorldQuery query, CancellationToken cancellationToken)
  {
    // TODO(fpion): permissions

    Dictionary<Guid, World> worlds = new(capacity: 2);

    if (query.Id.HasValue)
    {
      World? world = await _worldQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (world != null)
      {
        worlds[world.Id] = world;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueSlug))
    {
      World? world = await _worldQuerier.ReadAsync(query.UniqueSlug, cancellationToken);
      if (world != null)
      {
        worlds[world.Id] = world;
      }
    }

    if (worlds.Count > 1)
    {
      throw TooManyResultsException<World>.ExpectedSingle(worlds.Count);
    }

    return worlds.Values.SingleOrDefault();
  }
}
