using FluentValidation;
using Logitar.Identity.Domain.Shared;
using SkillCraft.Contracts.Worlds;
using SkillCraft.Domain.Shared;

namespace SkillCraft.Application.Worlds.Validators;

internal class CreateWorldValidator : AbstractValidator<CreateWorldPayload>
{
  public CreateWorldValidator()
  {
    RuleFor(x => x.UniqueSlug).SetValidator(new UniqueSlugValidator());
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).SetValidator(new DisplayNameValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).SetValidator(new DescriptionValidator()));
  }
}
