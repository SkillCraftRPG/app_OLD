using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Passwords.Validators;
using Logitar.Identity.Domain.Shared;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts.Validators;

internal class ResetPasswordValidator : AbstractValidator<ResetPasswordPayload>
{
  public ResetPasswordValidator(IPasswordSettings passwordSettings)
  {
    RuleFor(x => x.Locale).SetValidator(new LocaleValidator());

    When(x => !string.IsNullOrWhiteSpace(x.EmailAddress), () =>
    {
      RuleFor(x => x.EmailAddress).NotEmpty();
      RuleFor(x => x.Token).Empty();
      RuleFor(x => x.NewPassword).Empty();
    }).Otherwise(() =>
    {
      RuleFor(x => x.EmailAddress).Empty();
      RuleFor(x => x.Token).NotEmpty();
      RuleFor(x => x.NewPassword).NotNull();
      When(x => x.NewPassword != null, () => RuleFor(x => x.NewPassword!).SetValidator(new PasswordValidator(passwordSettings)));
    });
  }
}
