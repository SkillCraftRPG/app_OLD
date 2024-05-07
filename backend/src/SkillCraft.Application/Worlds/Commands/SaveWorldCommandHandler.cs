using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Domain.Worlds;
using SkillCraft.Domain.Worlds.Events;

namespace SkillCraft.Application.Worlds.Commands;

internal class SaveWorldCommandHandler : IRequestHandler<SaveWorldCommand>
{
  private readonly IWorldRepository _worldRepository;

  public SaveWorldCommandHandler(IWorldRepository worldRepository)
  {
    _worldRepository = worldRepository;
  }

  public async Task Handle(SaveWorldCommand command, CancellationToken cancellationToken)
  {
    WorldAggregate world = command.World;

    bool hasUniqueSlugChanged = false;
    foreach (DomainEvent change in world.Changes)
    {
      if (change is WorldCreatedEvent)
      {
        hasUniqueSlugChanged = true;
      }
    }

    if (hasUniqueSlugChanged)
    {
      WorldAggregate? other = await _worldRepository.LoadAsync(world.UniqueSlug, cancellationToken);
      if (other != null && !other.Equals(world))
      {
        throw new UniqueSlugAlreadyUsedException<WorldAggregate>(world.UniqueSlug, nameof(world.UniqueSlug));
      }
    }

    await _worldRepository.SaveAsync(world, cancellationToken);
  }
}
