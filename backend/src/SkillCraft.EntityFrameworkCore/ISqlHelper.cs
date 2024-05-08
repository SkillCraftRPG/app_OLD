using Logitar.Data;

namespace SkillCraft.EntityFrameworkCore;

public interface ISqlHelper
{
  IQueryBuilder QueryFrom(TableId table);
}
