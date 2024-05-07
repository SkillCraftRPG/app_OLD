using MediatR;

namespace SkillCraft.Application;

public interface IRequestPipeline
{
  Task<T> ExecuteAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default);
}
