namespace SkillCraft.Application.Storage;

public interface IStorageService
{
  Task EnsureEnoughAsync(Guid userId, int delta, CancellationToken cancellationToken = default);
}
