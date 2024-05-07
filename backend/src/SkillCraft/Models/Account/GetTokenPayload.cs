using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Models.Account;

public record GetTokenPayload : SignInPayload
{
  [JsonPropertyName("refresh_token")]
  public string? RefreshToken { get; set; }
}
