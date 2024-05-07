using Logitar.Portal.Contracts;
using MediatR;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts.Commands;

public record SignInCommand(SignInPayload Payload, IEnumerable<CustomAttribute> CustomAttributes) : IRequest<SignInCommandResult>;
