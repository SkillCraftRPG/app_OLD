using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;
using MediatR;

namespace SkillCraft.Infrastructure;

internal class EventBus : IEventBus
{
  private readonly IPublisher _publisher;

  public EventBus(IPublisher publisher)
  {
    _publisher = publisher;
  }

  public async Task PublishAsync(DomainEvent change, CancellationToken cancellationToken)
  {
    await _publisher.Publish(change, cancellationToken);
  }
}
