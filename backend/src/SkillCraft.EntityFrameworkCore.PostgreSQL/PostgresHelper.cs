using Logitar.Data;
using Logitar.Data.PostgreSQL;

namespace SkillCraft.EntityFrameworkCore.PostgreSQL;

internal class PostgresHelper : SqlHelper, ISqlHelper
{
  protected override ConditionalOperator CreateOperator(string pattern) => PostgresOperators.IsLikeInsensitive(pattern);

  public override IQueryBuilder QueryFrom(TableId table) => PostgresQueryBuilder.From(table);
}
