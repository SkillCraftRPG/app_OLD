using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Application.Permissions;

namespace SkillCraft.Application;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftApplication(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddTransient<IPermissionService, PermissionService>();
  }
}
