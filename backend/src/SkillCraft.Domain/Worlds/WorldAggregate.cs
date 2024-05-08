using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using SkillCraft.Contracts;
using SkillCraft.Domain.Shared;
using SkillCraft.Domain.Worlds.Events;

namespace SkillCraft.Domain.Worlds;

public class WorldAggregate : AggregateRoot
{
  private WorldUpdatedEvent _updatedEvent = new();

  public new WorldId Id => new(base.Id);

  private UniqueSlugUnit? _uniqueSlug = null;
  public UniqueSlugUnit UniqueSlug => _uniqueSlug ?? throw new InvalidOperationException($"The {nameof(UniqueSlug)} has not been initialized yet.");
  private DisplayNameUnit? _displayName = null;
  public DisplayNameUnit? DisplayName
  {
    get => _displayName;
    set
    {
      if (value != _displayName)
      {
        _displayName = value;
        _updatedEvent.DisplayName = new Change<DisplayNameUnit>(value);
      }
    }
  }
  private DescriptionUnit? _description = null;
  public DescriptionUnit? Description
  {
    get => _description;
    set
    {
      if (value != _description)
      {
        _description = value;
        _updatedEvent.Description = new Change<DescriptionUnit>(value);
      }
    }
  }

  public int Size => UniqueSlug.Value.Length + (DisplayName?.Value.Length ?? 0) + (Description?.Value.Length ?? 0);

  public WorldAggregate(AggregateId id) : base(id)
  {
  }

  public WorldAggregate(UniqueSlugUnit uniqueSlug, ActorId actorId = default, WorldId? id = null)
    : base((id ?? WorldId.NewId()).AggregateId)
  {
    Raise(new WorldCreatedEvent(uniqueSlug), actorId);
  }
  protected virtual void Apply(WorldCreatedEvent @event)
  {
    _uniqueSlug = @event.UniqueSlug;
  }

  public void Delete(ActorId actorId = default)
  {
    Raise(new WorldDeletedEvent(), actorId);
  }

  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, actorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(WorldUpdatedEvent @event)
  {
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueSlug.Value} | {base.ToString()}";
}
