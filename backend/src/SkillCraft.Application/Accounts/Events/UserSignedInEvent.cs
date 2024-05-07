using Logitar.Portal.Contracts.Sessions;
using MediatR;

namespace SkillCraft.Application.Accounts.Events;

public record UserSignedInEvent(Session Session) : INotification;
