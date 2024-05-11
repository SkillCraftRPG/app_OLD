using MediatR;
using SkillCraft.Application.Permissions;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Worlds.Queries;

internal class ReadWorldQueryHandler : IRequestHandler<ReadWorldQuery, World?>
{
  private readonly IPermissionService _permissionService;
  private readonly IWorldQuerier _worldQuerier;

  public ReadWorldQueryHandler(IPermissionService permissionService, IWorldQuerier worldQuerier)
  {
    _permissionService = permissionService;
    _worldQuerier = worldQuerier;
  }

  public async Task<World?> Handle(ReadWorldQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, World> worlds = new(capacity: 2);

    if (query.Id.HasValue)
    {
      World? world = await _worldQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (world != null && await _permissionService.IsAllowedToAsync(query, world, cancellationToken))
      {
        worlds[world.Id] = world;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueSlug))
    {
      World? world = await _worldQuerier.ReadAsync(query.UniqueSlug, cancellationToken);
      if (world != null && await _permissionService.IsAllowedToAsync(query, world, cancellationToken))
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
