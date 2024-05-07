using Logitar.Portal.Contracts.ApiKeys;

namespace SkillCraft.Application.Accounts;

public interface IApiKeyService
{
  Task<ApiKey> AuthenticateAsync(string xApiKey, CancellationToken cancellationToken = default);
}
