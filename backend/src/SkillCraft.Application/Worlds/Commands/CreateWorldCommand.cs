using MediatR;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Worlds.Commands;

public record CreateWorldCommand(CreateWorldPayload Payload) : Activity, IRequest<World>;
