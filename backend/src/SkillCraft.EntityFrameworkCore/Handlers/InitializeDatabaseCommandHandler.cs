using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SkillCraft.Infrastructure.Commands;

namespace SkillCraft.EntityFrameworkCore.Handlers;

internal class InitializeDatabaseCommandHandler : INotificationHandler<InitializeDatabaseCommand>
{
  private readonly bool _enableMigrations;
  private readonly EventContext _eventContext;
  private readonly SkillCraftContext _skillCraftContext;

  public InitializeDatabaseCommandHandler(IConfiguration configuration, EventContext eventContext, SkillCraftContext skillCraftContext)
  {
    _enableMigrations = configuration.GetValue<bool>("EnableMigrations");
    _eventContext = eventContext;
    _skillCraftContext = skillCraftContext;
  }

  public async Task Handle(InitializeDatabaseCommand _, CancellationToken cancellationToken)
  {
    if (_enableMigrations)
    {
      await _eventContext.Database.MigrateAsync(cancellationToken);
      await _skillCraftContext.Database.MigrateAsync(cancellationToken);
    }
  }
}
