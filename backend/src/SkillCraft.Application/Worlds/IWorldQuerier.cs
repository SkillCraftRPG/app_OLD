using SkillCraft.Contracts.Worlds;
using SkillCraft.Domain.Worlds;

namespace SkillCraft.Application.Worlds;

public interface IWorldQuerier
{
  Task<World> ReadAsync(WorldAggregate world, CancellationToken cancellationToken = default);
  Task<World?> ReadAsync(WorldId id, CancellationToken cancellationToken = default);
  Task<World?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
}
