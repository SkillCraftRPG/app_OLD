namespace SkillCraft.Contracts.Worlds;

public record CreateWorldPayload
{
  public string UniqueSlug { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public CreateWorldPayload() : this(string.Empty)
  {
  }

  public CreateWorldPayload(string uniqueSlug)
  {
    UniqueSlug = uniqueSlug;
  }
}
