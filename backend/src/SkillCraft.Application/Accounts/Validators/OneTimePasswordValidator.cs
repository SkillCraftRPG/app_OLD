using FluentValidation;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts.Validators;

internal class OneTimePasswordValidator : AbstractValidator<OneTimePasswordPayload>
{
  public OneTimePasswordValidator()
  {
    RuleFor(x => x.Id).NotEmpty();
    RuleFor(x => x.Code).NotEmpty();
  }
}
