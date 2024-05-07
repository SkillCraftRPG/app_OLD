using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.EntityFrameworkCore.Handlers;

namespace SkillCraft.EntityFrameworkCore.PostgreSQL;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "POSTGRESQLCONNSTR_SkillCraft";

  public static IServiceCollection AddSkillCraftWithEntityFrameworkCorePostgreSQL(this IServiceCollection services, IConfiguration configuration)
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
    return services.AddSkillCraftWithEntityFrameworkCorePostgreSQL(connectionString.Trim());
  }
  public static IServiceCollection AddSkillCraftWithEntityFrameworkCorePostgreSQL(this IServiceCollection services, string connectionString)
  {
    return services
      .AddDbContext<SkillCraftContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly("SkillCraft.EntityFrameworkCore.PostgreSQL")))
      .AddLogitarEventSourcingWithEntityFrameworkCorePostgreSQL(connectionString)
      .AddSkillCraftWithEntityFrameworkCore();
  }
}
