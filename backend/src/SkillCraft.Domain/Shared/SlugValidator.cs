using FluentValidation;

namespace SkillCraft.Domain.Shared;

public class SlugValidator : AbstractValidator<string>
{
  public SlugValidator()
  {
    RuleFor(x => x).Must(BeAValidSlug).WithErrorCode(nameof(SlugValidator))
      .WithMessage("'{PropertyName}' must be composed of non-empty alphanumeric words separated by hyphens (-).");
  }

  private static bool BeAValidSlug(string value)
  {
    return value.Split('-').All(value => !string.IsNullOrEmpty(value) && value.All(c => char.IsLetterOrDigit(c)));
  }
}
