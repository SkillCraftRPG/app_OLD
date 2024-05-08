using SkillCraft.Domain.Worlds.Events;

namespace SkillCraft.EntityFrameworkCore.Entities;

internal class WorldEntity : AggregateEntity
{
  public int WorldId { get; private set; }

  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => SkillCraftDb.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public WorldEntity(WorldCreatedEvent @event) : base(@event)
  {
    UniqueSlug = @event.UniqueSlug.Value;
  }

  private WorldEntity()
  {
  }

  public void Update(WorldUpdatedEvent @event)
  {
    base.Update(@event);

    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }
  }
}
