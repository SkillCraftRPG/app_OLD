using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Passwords.Validators;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts.Validators;

internal class CompleteProfileValidator : SaveProfileValidatorBase<CompleteProfilePayload>
{
  public CompleteProfileValidator(IPasswordSettings passwordSettings) : base()
  {
    RuleFor(x => x.Token).NotEmpty();

    When(x => x.Password != null, () => RuleFor(x => x.Password!).SetValidator(new PasswordValidator(passwordSettings)));
    RuleFor(x => x.MultiFactorAuthenticationMode).IsInEnum();
  }
}
