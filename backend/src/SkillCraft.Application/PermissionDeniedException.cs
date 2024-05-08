using Logitar;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Errors;

namespace SkillCraft.Application;

public class PermissionDeniedException : Exception
{
  private const string ErrorMessage = "The specified permission was denied to the specified actor.";

  public string Actor
  {
    get => (string)Data[nameof(Actor)]!;
    private set => Data[nameof(Actor)] = value;
  }
  public string Permission
  {
    get => (string)Data[nameof(Permission)]!;
    private set => Data[nameof(Permission)] = value;
  }

  public virtual Error Error => new(code: "PermissionDenied", ErrorMessage);

  public PermissionDeniedException(Actor actor, string permission)
    : base(BuildMessage(actor, permission))
  {
    Actor = actor.ToString();
    Permission = permission;
  }

  private static string BuildMessage(Actor actor, string permission) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(Actor), actor.ToString())
    .AddData(nameof(Permission), permission)
    .Build();
}
