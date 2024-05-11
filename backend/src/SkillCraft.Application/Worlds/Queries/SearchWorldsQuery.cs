﻿using MediatR;
using SkillCraft.Contracts.Search;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Worlds.Queries;

public record SearchWorldsQuery(SearchWorldsPayload Payload) : Activity, IRequest<SearchResults<World>>;
