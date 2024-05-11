using SkillCraft.Application.Worlds.Commands;
using SkillCraft.Application.Worlds.Queries;
using SkillCraft.Contracts.Worlds;
using SkillCraft.Domain.Worlds;

namespace SkillCraft.Application.Permissions;

internal class PermissionService : IPermissionService // TODO(fpion): refactor
{
  private const int WorldLimit = 1; // TODO(fpion): only one world?

  private readonly IWorldRepository _worldRepository;

  public PermissionService(IWorldRepository worldRepository)
  {
    _worldRepository = worldRepository;
  }

  public async Task EnsureCanAsync(CreateWorldCommand command, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<WorldAggregate> worlds = await _worldRepository.LoadAsync(command.ActorId, cancellationToken);
    if (worlds.Count >= WorldLimit)
    {
      throw new PermissionDeniedException(command.Actor, "CreateWorld");
    }
  }

  public Task<bool> IsAllowedToAsync(ReadWorldQuery query, World world, CancellationToken cancellationToken)
  {
    return Task.FromResult(query.UserId == world.CreatedBy.Id); // TODO(fpion): OwnerId, Members
  }
}
