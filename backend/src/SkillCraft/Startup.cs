using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Client;
using Microsoft.AspNetCore.Authorization;
using SkillCraft.Application;
using SkillCraft.Authentication;
using SkillCraft.Constants;
using SkillCraft.EntityFrameworkCore;
using SkillCraft.EntityFrameworkCore.PostgreSQL;
using SkillCraft.EntityFrameworkCore.SqlServer;
using SkillCraft.Extensions;
using SkillCraft.Filters;
using SkillCraft.Infrastructure;
using SkillCraft.Settings;

namespace SkillCraft;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;
  private readonly string[] _authenticationSchemes;
  private readonly bool _enableOpenApi;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
    _authenticationSchemes = Schemes.GetEnabled(configuration);
    _enableOpenApi = configuration.GetValue<bool>("EnableOpenApi");
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddControllers(options => options.Filters.Add<ExceptionHandling>())
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    CorsSettings corsSettings = _configuration.GetSection("Cors").Get<CorsSettings>() ?? new();
    services.AddSingleton(corsSettings);
    services.AddCors(corsSettings);

    BearerTokenSettings bearerTokenSettings = _configuration.GetSection("BearerToken").Get<BearerTokenSettings>() ?? new();
    services.AddSingleton(bearerTokenSettings);
    services.AddSingleton<IBearerTokenService, BearerTokenService>();

    //AuthenticationBuilder authenticationBuilder = services.AddAuthentication()
    //      .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Schemes.ApiKey, options => { })
    //      .AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(Schemes.Bearer, options => { })
    //      .AddScheme<SessionAuthenticationOptions, SessionAuthenticationHandler>(Schemes.Session, options => { });
    //if (_authenticationSchemes.Contains(Schemes.Basic))
    //{
    //  authenticationBuilder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(Schemes.Basic, options => { });
    //}

    services.AddAuthorizationBuilder().SetDefaultPolicy(new AuthorizationPolicyBuilder(_authenticationSchemes)
      .RequireAuthenticatedUser()
      .Build());

    CookiesSettings cookiesSettings = _configuration.GetSection("Cookies").Get<CookiesSettings>() ?? new();
    services.AddSingleton(cookiesSettings);
    services.AddDistributedMemoryCache();
    services.AddSession(options =>
    {
      options.Cookie.SameSite = cookiesSettings.Session.SameSite;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

    services.AddApplicationInsightsTelemetry();
    IHealthChecksBuilder healthChecks = services.AddHealthChecks();

    if (_enableOpenApi)
    {
      services.AddOpenApi();
    }

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        services.AddSkillCraftWithEntityFrameworkCorePostgreSQL(_configuration);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<SkillCraftContext>();
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        services.AddSkillCraftWithEntityFrameworkCoreSqlServer(_configuration);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<SkillCraftContext>();
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddLogitarPortalClient(_configuration);
    services.AddTransient<IRequestPipeline, HttpRequestPipeline>();
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (_enableOpenApi)
    {
      builder.UseOpenApi();
    }

    builder.UseHttpsRedirection();
    builder.UseCors();
    builder.UseSession();
    //builder.UseMiddleware<RenewSession>();
    builder.UseAuthentication();
    builder.UseAuthorization();

    if (builder is WebApplication application)
    {
      application.MapControllers();
      application.MapHealthChecks("/health");
    }
  }
}
