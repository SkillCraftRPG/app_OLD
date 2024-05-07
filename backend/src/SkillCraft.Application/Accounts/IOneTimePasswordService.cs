using Logitar.Portal.Contracts.Passwords;
using Logitar.Portal.Contracts.Users;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts;

public interface IOneTimePasswordService
{
  Task<OneTimePassword> CreateAsync(User user, string purpose, CancellationToken cancellationToken = default);
  Task<OneTimePassword> CreateAsync(User user, string purpose, Phone? phone, CancellationToken cancellationToken = default);
  Task<OneTimePassword> ValidateAsync(OneTimePasswordPayload payload, string purpose, CancellationToken cancellationToken = default);
}
