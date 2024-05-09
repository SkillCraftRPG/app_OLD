using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Infrastructure.Converters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Application;
using SkillCraft.Application.Accounts;
using SkillCraft.Application.Caching;
using SkillCraft.Infrastructure.Caching;
using SkillCraft.Infrastructure.Converters;
using SkillCraft.Infrastructure.IdentityServices;
using SkillCraft.Infrastructure.Settings;

namespace SkillCraft.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingInfrastructure()
      .AddSkillCraftApplication()
      .AddIdentityServices()
      .AddMemoryCache()
      .AddSingleton(InitializeCachingSettings)
      .AddSingleton<ICacheService, CacheService>()
      .AddSingleton<IEventSerializer>(BuildEventSerializer)
      .AddTransient<IEventBus, EventBus>();
  }

  private static IServiceCollection AddIdentityServices(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyService, ApiKeyService>()
      .AddTransient<IMessageService, MessageService>()
      .AddTransient<IOneTimePasswordService, OneTimePasswordService>()
      .AddTransient<IRealmService, RealmService>()
      .AddTransient<ISessionService, SessionService>()
      .AddTransient<ITokenService, TokenService>()
      .AddTransient<IUserService, UserService>();
  }

  private static EventSerializer BuildEventSerializer(IServiceProvider serviceProvider)
  {
    EventSerializer eventSerializer = new();

    eventSerializer.RegisterConverter(new DescriptionConverter());
    eventSerializer.RegisterConverter(new DisplayNameConverter());
    eventSerializer.RegisterConverter(new UniqueSlugConverter());
    eventSerializer.RegisterConverter(new WorldIdConverter());

    return eventSerializer;
  }

  private static CachingSettings InitializeCachingSettings(IServiceProvider serviceProvider)
  {
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetSection("Caching").Get<CachingSettings>() ?? new();
  }
}
