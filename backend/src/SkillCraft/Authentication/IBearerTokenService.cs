using Logitar.Portal.Contracts.Sessions;
using SkillCraft.Models.Account;

namespace SkillCraft.Authentication;

public interface IBearerTokenService
{
  TokenResponse GetTokenResponse(Session session);
  ClaimsPrincipal ValidateToken(string token);
}
