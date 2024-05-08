using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Domain.Worlds.Events;
using SkillCraft.EntityFrameworkCore.Entities;

namespace SkillCraft.EntityFrameworkCore.Handlers;

internal static class Worlds
{
  public class WorldCreatedEventHandler : INotificationHandler<WorldCreatedEvent>
  {
    private readonly SkillCraftContext _context;

    public WorldCreatedEventHandler(SkillCraftContext context)
    {
      _context = context;
    }

    public async Task Handle(WorldCreatedEvent @event, CancellationToken cancellationToken)
    {
      WorldEntity? world = await _context.Worlds.AsNoTracking()
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (world == null)
      {
        world = new(@event);

        _context.Worlds.Add(world);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class WorldDeletedEventHandler : INotificationHandler<WorldDeletedEvent>
  {
    private readonly SkillCraftContext _context;

    public WorldDeletedEventHandler(SkillCraftContext context)
    {
      _context = context;
    }

    public async Task Handle(WorldDeletedEvent @event, CancellationToken cancellationToken)
    {
      WorldEntity? world = await _context.Worlds
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (world != null)
      {
        _context.Worlds.Remove(world);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class WorldUpdatedEventHandler : INotificationHandler<WorldUpdatedEvent>
  {
    private readonly SkillCraftContext _context;

    public WorldUpdatedEventHandler(SkillCraftContext context)
    {
      _context = context;
    }

    public async Task Handle(WorldUpdatedEvent @event, CancellationToken cancellationToken)
    {
      WorldEntity world = await _context.Worlds
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The world entity 'AggregateId={@event.AggregateId}' could not be found.");

      world.Update(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
