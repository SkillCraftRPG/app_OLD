using Logitar;
using Logitar.Portal.Contracts.Errors;

namespace SkillCraft.Application;

public class NotEnoughStorageRemainingException : Exception
{
  private const string ErrorMessage = "There is not enough storage remaining.";

  public Guid UserId
  {
    get => (Guid)Data[nameof(UserId)]!;
    private set => Data[nameof(UserId)] = value;
  }
  public long RemainingStorage
  {
    get => (long)Data[nameof(RemainingStorage)]!;
    private set => Data[nameof(RemainingStorage)] = value;
  }
  public int RequiredStorage
  {
    get => (int)Data[nameof(RequiredStorage)]!;
    private set => Data[nameof(RequiredStorage)] = value;
  }

  public virtual Error Error => new(this.GetErrorCode(), ErrorMessage);

  public NotEnoughStorageRemainingException(Guid userId, long remainingStorage, int requiredStorage)
    : base(BuildMessage(userId, remainingStorage, requiredStorage))
  {
    if (requiredStorage < 0 || requiredStorage <= remainingStorage)
    {
      throw new ArgumentOutOfRangeException(nameof(requiredStorage));
    }

    UserId = userId;
    RemainingStorage = remainingStorage;
    RequiredStorage = requiredStorage;
  }

  private static string BuildMessage(Guid userId, long remainingStorage, int requiredStorage) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), userId)
    .AddData(nameof(RemainingStorage), remainingStorage)
    .AddData(nameof(RequiredStorage), requiredStorage)
    .Build();
}
