using Logitar.EventSourcing;
using MediatR;
using SkillCraft.Domain.Shared;

namespace SkillCraft.Domain.Worlds.Events;

public record WorldCreatedEvent(UniqueSlugUnit UniqueSlug) : DomainEvent, INotification;
