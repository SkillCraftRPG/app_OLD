using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.EntityFrameworkCore.Actors;
using SkillCraft.Infrastructure;

namespace SkillCraft.EntityFrameworkCore.Handlers;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftWithEntityFrameworkCore(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddSkillCraftInfrastructure()
      //.AddQueriers()
      //.AddRepositories()
      .AddTransient<IActorService, ActorService>();
  }

  //private static IServiceCollection AddQueriers(this IServiceCollection services)
  //{
  //  return services.AddTransient<IProjectQuerier, ProjectQuerier>();
  //}

  //private static IServiceCollection AddRepositories(this IServiceCollection services)
  //{
  //  return services.AddTransient<IProjectRepository, ProjectRepository>();
  //}
}
