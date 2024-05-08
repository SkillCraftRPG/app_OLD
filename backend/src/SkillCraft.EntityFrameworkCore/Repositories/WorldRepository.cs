using Logitar;
using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Domain.Shared;
using SkillCraft.Domain.Worlds;

namespace SkillCraft.EntityFrameworkCore.Repositories;

internal class WorldRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IWorldRepository
{
  private static readonly string AggregateType = typeof(WorldAggregate).GetNamespaceQualifiedName();

  private readonly ISqlHelper _sqlHelper;

  public WorldRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    _sqlHelper = sqlHelper;
  }

  public async Task<WorldAggregate?> LoadAsync(WorldId id, CancellationToken cancellationToken)
  {
    return await base.LoadAsync<WorldAggregate>(id.AggregateId, cancellationToken);
  }

  public async Task<WorldAggregate?> LoadAsync(UniqueSlugUnit uniqueSlug, CancellationToken cancellationToken)
  {
    IQuery query = _sqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(SkillCraftDb.Worlds.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(SkillCraftDb.Worlds.UniqueSlugNormalized, Operators.IsEqualTo(SkillCraftDb.Normalize(uniqueSlug.Value)))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<WorldAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public async Task<IReadOnlyCollection<WorldAggregate>> LoadAsync(ActorId owner, CancellationToken cancellationToken)
  {
    IQuery query = _sqlHelper.QueryFrom(EventDb.Events.Table)
      .Join(SkillCraftDb.Worlds.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(SkillCraftDb.Worlds.CreatedBy, Operators.IsEqualTo(owner.Value))
      .SelectAll(EventDb.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<WorldAggregate>(events.Select(EventSerializer.Deserialize)).ToArray();
  }

  public async Task SaveAsync(WorldAggregate world, CancellationToken cancellationToken)
  {
    await base.SaveAsync(world, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<WorldAggregate> worlds, CancellationToken cancellationToken)
  {
    await base.SaveAsync(worlds, cancellationToken);
  }
}
