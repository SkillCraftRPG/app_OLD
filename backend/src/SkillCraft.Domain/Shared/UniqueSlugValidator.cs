using FluentValidation;

namespace SkillCraft.Domain.Shared;

public class UniqueSlugValidator : AbstractValidator<string>
{
  public UniqueSlugValidator()
  {
    RuleFor(x => x).NotEmpty().MaximumLength(UniqueSlugUnit.MaximumLength).SetValidator(new SlugValidator());
  }
}
