using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Application;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftApplication(this IServiceCollection services)
  {
    return services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
  }
}
