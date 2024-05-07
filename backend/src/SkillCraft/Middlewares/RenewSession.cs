using Logitar.Portal.Contracts.Sessions;
using SkillCraft.Application.Accounts;
using SkillCraft.Constants;
using SkillCraft.Extensions;

namespace SkillCraft.Middlewares;

internal class RenewSession
{
  private readonly RequestDelegate _next;

  public RenewSession(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
  {
    if (!context.GetSessionId().HasValue)
    {
      if (context.Request.Cookies.TryGetValue(Cookies.RefreshToken, out string? refreshToken) && refreshToken != null)
      {
        try
        {
          Session session = await sessionService.RenewAsync(refreshToken, context.GetSessionCustomAttributes());
          context.SignIn(session);
        }
        catch (Exception)
        {
          context.Response.Cookies.Delete(Cookies.RefreshToken);
        }
      }
    }

    await _next(context);
  }
}
