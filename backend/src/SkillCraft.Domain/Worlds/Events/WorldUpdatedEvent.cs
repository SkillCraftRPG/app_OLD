using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;
using SkillCraft.Contracts;

namespace SkillCraft.Domain.Worlds.Events;

public record WorldUpdatedEvent : DomainEvent, INotification
{
  public Change<DisplayNameUnit>? DisplayName { get; set; }
  public Change<DescriptionUnit>? Description { get; set; }

  public bool HasChanges => DisplayName != null || Description != null;
}
