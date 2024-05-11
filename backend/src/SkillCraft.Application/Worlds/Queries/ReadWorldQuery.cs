using MediatR;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Worlds.Queries;

public record ReadWorldQuery(Guid? Id, string? UniqueSlug) : Activity, IRequest<World?>;
