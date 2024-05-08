using Microsoft.EntityFrameworkCore;
using SkillCraft.Application;
using SkillCraft.Application.Storage;
using SkillCraft.EntityFrameworkCore.Entities;

namespace SkillCraft.EntityFrameworkCore.Storage;

internal class StorageService : IStorageService
{
  private const long DefaultStorage = 5 * 1024 * 1024; // NOTE(fpion): 5 MB

  private readonly DbSet<StorageSummaryEntity> _summaries;

  public StorageService(SkillCraftContext context)
  {
    _summaries = context.StorageSummaries;
  }

  public async Task EnsureEnoughAsync(Guid userId, int delta, CancellationToken cancellationToken)
  {
    if (delta > 0)
    {
      StorageSummaryEntity? summary = await _summaries.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
      long remaining = summary?.Remaining ?? DefaultStorage;
      if (delta > remaining)
      {
        throw new NotEnoughStorageRemainingException(userId, remaining, delta);
      }
    }
  }
}
