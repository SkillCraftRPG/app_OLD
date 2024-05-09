using Logitar.Data;
using Logitar.Data.SqlServer;

namespace SkillCraft.EntityFrameworkCore.SqlServer;

internal class SqlServerHelper : SqlHelper, ISqlHelper
{
  public override IQueryBuilder QueryFrom(TableId table) => SqlServerQueryBuilder.From(table);
}
