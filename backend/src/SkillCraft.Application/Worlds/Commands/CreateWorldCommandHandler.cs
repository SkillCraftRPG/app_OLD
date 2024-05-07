using FluentValidation;
using Logitar.Identity.Domain.Shared;
using MediatR;
using SkillCraft.Application.Permissions;
using SkillCraft.Application.Worlds.Validators;
using SkillCraft.Contracts.Worlds;
using SkillCraft.Domain.Shared;
using SkillCraft.Domain.Worlds;

namespace SkillCraft.Application.Worlds.Commands;

internal class CreateWorldCommandHandler : IRequestHandler<CreateWorldCommand, World>
{
  private readonly IPermissionService _permissionService;
  private readonly ISender _sender;
  private readonly IWorldQuerier _worldQuerier;

  public CreateWorldCommandHandler(IPermissionService permissionService, ISender sender, IWorldQuerier worldQuerier)
  {
    _permissionService = permissionService;
    _sender = sender;
    _worldQuerier = worldQuerier;
  }

  public async Task<World> Handle(CreateWorldCommand command, CancellationToken cancellationToken)
  {
    CreateWorldPayload payload = command.Payload;
    new CreateWorldValidator().ValidateAndThrow(payload);

    await _permissionService.EnsureCanAsync(command, cancellationToken);

    UniqueSlugUnit uniqueSlug = new(payload.UniqueSlug);
    WorldAggregate world = new(uniqueSlug, command.ActorId)
    {
      DisplayName = DisplayNameUnit.TryCreate(payload.DisplayName),
      Description = DescriptionUnit.TryCreate(payload.Description)
    };
    world.Update(command.ActorId);

    // TODO(fpion): ensure user has enough space
    await _sender.Send(new SaveWorldCommand(world), cancellationToken);
    // TODO(fpion): allocate user space

    return await _worldQuerier.ReadAsync(world, cancellationToken);
  }
}
