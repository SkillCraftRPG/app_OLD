using Logitar.Portal.Contracts;
using MediatR;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts.Commands;

public record ResetPasswordCommand(ResetPasswordPayload Payload, IEnumerable<CustomAttribute> CustomAttributes) : IRequest<ResetPasswordResult>;
