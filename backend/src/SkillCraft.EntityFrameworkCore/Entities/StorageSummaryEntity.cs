namespace SkillCraft.EntityFrameworkCore.Entities;

internal class StorageSummaryEntity // TODO(fpion): private fields
{
  public int StorageSummaryId { get; set; }

  public string ActorId { get; set; } = string.Empty;
  public Guid UserId { get; set; }

  public long Allocated { get; set; }
  public long Used { get; set; }
  public long Remaining
  {
    get => Allocated - Used;
    set { }
  }

  public StorageSummaryEntity()
  {
  }
}
