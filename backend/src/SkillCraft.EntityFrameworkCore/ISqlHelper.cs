using Logitar.Data;
using SkillCraft.Contracts.Search;

namespace SkillCraft.EntityFrameworkCore;

public interface ISqlHelper
{
  void ApplyTextSearch(IQueryBuilder query, TextSearch? search, params ColumnId[] columns);
  IQueryBuilder QueryFrom(TableId table);
}
