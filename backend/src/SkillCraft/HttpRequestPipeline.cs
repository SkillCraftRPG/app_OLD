using MediatR;
using SkillCraft.Application;
using SkillCraft.Extensions;

namespace SkillCraft;

internal class HttpRequestPipeline : IRequestPipeline
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ISender _sender;

  public HttpRequestPipeline(IHttpContextAccessor httpContextAccessor, ISender sender)
  {
    _httpContextAccessor = httpContextAccessor;
    _sender = sender;
  }

  public async Task<T> ExecuteAsync<T>(IRequest<T> request, CancellationToken cancellationToken)
  {
    if (request is IActivity activity)
    {
      ActivityContext context = GetActivityContext();
      activity.Contextualize(context);
    }

    return await _sender.Send(request, cancellationToken);
  }

  private ActivityContext GetActivityContext()
  {
    HttpContext? context = _httpContextAccessor.HttpContext;
    return new ActivityContext(context?.GetApiKey(), context?.GetUser(), context?.GetSession());
  }
}
