using SkillCraft.Application.Worlds.Commands;
using SkillCraft.Domain.Worlds;

namespace SkillCraft.Application.Permissions;

internal class PermissionService : IPermissionService
{
  private readonly IWorldRepository _worldRepository;

  public PermissionService(IWorldRepository worldRepository)
  {
    _worldRepository = worldRepository;
  }

  public async Task EnsureCanAsync(CreateWorldCommand command, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<WorldAggregate> worlds = await _worldRepository.LoadAsync(command.ActorId, cancellationToken);
    if (worlds.Count > 0)
    {
      throw new PermissionDeniedException(command.Actor, "CreateWorld");
    }
  }
}
