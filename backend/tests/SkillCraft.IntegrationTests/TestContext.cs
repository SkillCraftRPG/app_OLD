using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using SkillCraft.Application;

namespace SkillCraft;

internal record TestContext
{
  public ApiKey? ApiKey { get; set; }
  public User? User { get; set; }
  public Session? Session { get; set; }

  public ActivityContext GetActivityContext() => new(ApiKey, User, Session);
}
