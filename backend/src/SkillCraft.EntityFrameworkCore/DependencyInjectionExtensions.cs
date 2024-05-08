using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Application.Worlds;
using SkillCraft.Domain.Worlds;
using SkillCraft.EntityFrameworkCore.Actors;
using SkillCraft.EntityFrameworkCore.Queriers;
using SkillCraft.EntityFrameworkCore.Repositories;
using SkillCraft.Infrastructure;

namespace SkillCraft.EntityFrameworkCore;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftWithEntityFrameworkCore(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddSkillCraftInfrastructure()
      .AddQueriers()
      .AddRepositories()
      .AddTransient<IActorService, ActorService>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services.AddTransient<IWorldQuerier, WorldQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services.AddTransient<IWorldRepository, WorldRepository>();
  }
}
