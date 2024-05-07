using SkillCraft.Application.Worlds.Commands;

namespace SkillCraft.Application.Permissions;

public interface IPermissionService
{
  Task EnsureCanAsync(CreateWorldCommand command, CancellationToken cancellationToken = default);
}
