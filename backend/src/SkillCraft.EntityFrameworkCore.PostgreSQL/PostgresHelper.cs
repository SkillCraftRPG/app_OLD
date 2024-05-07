using Logitar.Data;
using Logitar.Data.PostgreSQL;

namespace SkillCraft.EntityFrameworkCore.PostgreSQL;

internal class PostgresHelper : ISqlHelper
{
  public IQueryBuilder QueryFrom(TableId table) => PostgresQueryBuilder.From(table);
}
