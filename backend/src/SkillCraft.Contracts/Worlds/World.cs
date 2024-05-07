using Logitar.Portal.Contracts;

namespace SkillCraft.Contracts.Worlds;

public class World : Aggregate
{
  public string UniqueSlug { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public World() : this(string.Empty)
  {
  }

  public World(string uniqueSlug)
  {
    UniqueSlug = uniqueSlug;
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
