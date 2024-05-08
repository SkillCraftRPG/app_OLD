namespace SkillCraft.Domain.Shared;

public record UniqueSlugUnit
{
  public const int MaximumLength = 32;

  public string Value { get; }

  public UniqueSlugUnit(string value)
  {
    Value = value.Trim();
  }

  public static UniqueSlugUnit? TryCreate(string? value)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value);
  }
}
