using Logitar.EventSourcing;
using SkillCraft.Domain.Shared;

namespace SkillCraft.Domain.Worlds;

public interface IWorldRepository
{
  Task<WorldAggregate?> LoadAsync(WorldId id, CancellationToken cancellationToken = default);
  Task<WorldAggregate?> LoadAsync(UniqueSlugUnit uniqueSlug, CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<WorldAggregate>> LoadAsync(ActorId owner, CancellationToken cancellationToken = default);

  Task SaveAsync(WorldAggregate world, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<WorldAggregate> worlds, CancellationToken cancellationToken = default);
}
