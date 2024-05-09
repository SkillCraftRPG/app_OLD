using SkillCraft.Contracts.Search;

namespace SkillCraft.Contracts.Worlds;

public record SearchWorldsPayload : SearchPayload
{
  public new List<WorldSortOption>? Sort { get; set; }
}
