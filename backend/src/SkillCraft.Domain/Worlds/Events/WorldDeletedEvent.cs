using Logitar.EventSourcing;
using MediatR;

namespace SkillCraft.Domain.Worlds.Events;

public record WorldDeletedEvent : DomainEvent, INotification;
