using Logitar.EventSourcing;

namespace SkillCraft.Domain.Worlds;

public record WorldId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  internal WorldId(string id) : this(new AggregateId(id))
  {
  }
  internal WorldId(Guid id) : this(new AggregateId(id))
  {
  }
  internal WorldId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static WorldId NewId() => new(AggregateId.NewId());

  public static WorldId? TryCreate(string? value)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value);
  }

  public Guid ToGuid() => AggregateId.ToGuid();
}
