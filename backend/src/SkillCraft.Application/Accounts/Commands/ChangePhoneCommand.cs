using MediatR;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Application.Accounts.Commands;

public record ChangePhoneCommand(ChangePhonePayload Payload) : Activity, IRequest<ChangePhoneResult>;
