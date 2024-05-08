using Logitar.Data;
using Logitar.Data.SqlServer;

namespace SkillCraft.EntityFrameworkCore.SqlServer;

internal class SqlServerHelper : ISqlHelper
{
  public IQueryBuilder QueryFrom(TableId table) => SqlServerQueryBuilder.From(table);
}
