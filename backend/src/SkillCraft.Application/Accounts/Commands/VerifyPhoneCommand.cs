using MediatR;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts.Commands;

public record VerifyPhoneCommand(VerifyPhonePayload Payload) : IRequest<VerifyPhoneResult>;
