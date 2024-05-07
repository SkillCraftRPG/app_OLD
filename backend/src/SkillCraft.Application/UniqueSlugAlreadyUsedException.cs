using Logitar;
using SkillCraft.Contracts.Errors;
using SkillCraft.Domain.Shared;

namespace SkillCraft.Application;

public class UniqueSlugAlreadyUsedException : ConflictException
{
  private const string ErrorMessage = "The specified unique slug is already used.";

  public string TypeName
  {
    get => (string)Data[nameof(TypeName)]!;
    private set => Data[nameof(TypeName)] = value;
  }
  public UniqueSlugUnit UniqueSlug
  {
    get => new((string)Data[nameof(UniqueSlug)]!);
    private set => Data[nameof(UniqueSlug)] = value.Value;
  }
  public string? PropertyName
  {
    get => (string?)Data[nameof(PropertyName)];
    private set => Data[nameof(PropertyName)] = value;
  }

  public override PropertyError Error => new(this.GetErrorCode(), ErrorMessage, PropertyName, UniqueSlug.Value);

  public UniqueSlugAlreadyUsedException(Type type, UniqueSlugUnit uniqueSlug, string? propertyName = null)
    : base(BuildMessage(type, uniqueSlug, propertyName))
  {
    TypeName = type.GetNamespaceQualifiedName();
    UniqueSlug = uniqueSlug;
    PropertyName = propertyName;
  }

  private static string BuildMessage(Type type, UniqueSlugUnit uniqueSlug, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TypeName), type.GetNamespaceQualifiedName())
    .AddData(nameof(UniqueSlug), uniqueSlug.Value)
    .AddData(nameof(PropertyName), propertyName, "<null>")
    .Build();
}

public class UniqueSlugAlreadyUsedException<T> : UniqueSlugAlreadyUsedException
{
  public UniqueSlugAlreadyUsedException(UniqueSlugUnit uniqueSlug, string? propertyName = null)
    : base(typeof(T), uniqueSlug, propertyName)
  {
  }
}
