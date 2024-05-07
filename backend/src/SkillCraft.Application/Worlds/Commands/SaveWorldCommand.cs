using MediatR;
using SkillCraft.Domain.Worlds;

namespace SkillCraft.Application.Worlds.Commands;

internal record SaveWorldCommand(WorldAggregate World) : IRequest;
