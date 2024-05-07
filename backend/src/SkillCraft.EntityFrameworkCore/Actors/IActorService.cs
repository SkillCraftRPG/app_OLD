﻿using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;

namespace SkillCraft.EntityFrameworkCore.Actors;

public interface IActorService
{
  Task<IReadOnlyCollection<Actor>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken = default);
}
