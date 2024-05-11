using SkillCraft.Application.Worlds.Commands;
using SkillCraft.Application.Worlds.Queries;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Permissions;

public interface IPermissionService
{
  Task EnsureCanAsync(CreateWorldCommand command, CancellationToken cancellationToken = default);
  Task<bool> IsAllowedToAsync(ReadWorldQuery query, World world, CancellationToken cancellationToken = default);
}
