using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.EntityFrameworkCore.Entities;

namespace SkillCraft.EntityFrameworkCore.Configurations;

internal class StorageSummaryConfiguration : IEntityTypeConfiguration<StorageSummaryEntity>
{
  public void Configure(EntityTypeBuilder<StorageSummaryEntity> builder)
  {
    builder.ToTable(nameof(SkillCraftContext.StorageSummaries));
    builder.HasKey(x => x.StorageSummaryId);

    builder.HasIndex(x => x.ActorId).IsUnique();
    builder.HasIndex(x => x.UserId).IsUnique();
    builder.HasIndex(x => x.Allocated);
    builder.HasIndex(x => x.Used);
    builder.HasIndex(x => x.Remaining);

    builder.Property(x => x.ActorId).HasMaxLength(ActorId.MaximumLength);
  }
}
