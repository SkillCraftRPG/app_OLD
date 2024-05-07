using Logitar.EventSourcing.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.EntityFrameworkCore.SqlServer;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "SQLCONNSTR_SkillCraft";

  public static IServiceCollection AddSkillCraftWithEntityFrameworkCoreSqlServer(this IServiceCollection services, IConfiguration configuration)
  {
    string? connectionString = Environment.GetEnvironmentVariable(ConfigurationKey);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      connectionString = configuration.GetValue<string>(ConfigurationKey);
    }
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new ArgumentException($"The configuration '{ConfigurationKey}' could not be found.", nameof(configuration));
    }
    return services.AddSkillCraftWithEntityFrameworkCoreSqlServer(connectionString.Trim());
  }
  public static IServiceCollection AddSkillCraftWithEntityFrameworkCoreSqlServer(this IServiceCollection services, string connectionString)
  {
    return services
      .AddDbContext<SkillCraftContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SkillCraft.EntityFrameworkCore.SqlServer")))
      .AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString)
      .AddSkillCraftWithEntityFrameworkCore()
      .AddSingleton<ISqlHelper, SqlServerHelper>();
  }
}
