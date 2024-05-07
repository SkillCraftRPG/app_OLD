namespace SkillCraft.Contracts.Accounts;

public record OneTimePasswordPayload
{
  public Guid Id { get; set; }
  public string Code { get; set; }

  public OneTimePasswordPayload() : this(Guid.Empty, string.Empty)
  {
  }

  public OneTimePasswordPayload(Guid id, string code)
  {
    Id = id;
    Code = code;
  }
}
