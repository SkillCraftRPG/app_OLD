using Logitar.Portal.Contracts.Realms;

namespace SkillCraft.Application.Accounts;

public interface IRealmService
{
  Task<Realm> FindAsync(CancellationToken cancellationToken = default);
}
