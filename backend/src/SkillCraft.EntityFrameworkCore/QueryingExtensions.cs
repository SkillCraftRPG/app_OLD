using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace SkillCraft.EntityFrameworkCore;

internal static class QueryingExtensions
{
  public static IQueryBuilder ApplyIdFilter(this IQueryBuilder query, IEnumerable<Guid>? ids, ColumnId column)
  {
    if (ids != null && ids.Any())
    {
      string[] aggregateIds = ids.Distinct().Select(id => new AggregateId(id).Value).ToArray();
      query.Where(column, Operators.IsIn(aggregateIds));
    }

    return query;
  }

  public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int? skip, int? limit)
  {
    if (skip > 0)
    {
      query = query.Skip(skip.Value);
    }

    if (limit > 0)
    {
      query = query.Take(limit.Value);
    }

    return query;
  }

  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQueryBuilder builder) where T : class
  {
    return entities.FromQuery(builder.Build());
  }
  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQuery query) where T : class
  {
    return entities.FromSqlRaw(query.Text, query.Parameters.ToArray());
  }
}
