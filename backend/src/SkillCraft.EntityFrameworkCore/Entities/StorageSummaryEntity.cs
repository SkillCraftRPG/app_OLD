namespace SkillCraft.EntityFrameworkCore.Entities;

internal class StorageSummaryEntity
{
  public int StorageSummaryId { get; private set; }

  public string ActorId { get; private set; } = string.Empty;
  public Guid UserId { get; private set; }

  public long Allocated { get; private set; }
  public long Used { get; private set; }
  public long Remaining
  {
    get => Allocated - Used;
    private set { }
  }

  private StorageSummaryEntity()
  {
  }
}
