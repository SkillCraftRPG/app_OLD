using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace SkillCraft.Application;

public record ActivityContext(ApiKey? ApiKey, User? User, Session? Session);
