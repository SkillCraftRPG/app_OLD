namespace SkillCraft.Infrastructure.Settings;

internal record CachingSettings
{
  public TimeSpan? ActorLifetime { get; set; }
}
