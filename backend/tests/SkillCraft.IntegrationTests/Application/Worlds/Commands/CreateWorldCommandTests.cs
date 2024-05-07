using FluentValidation.Results;
using Logitar;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Contracts.Worlds;
using SkillCraft.Domain.Shared;
using SkillCraft.Domain.Worlds;
using SkillCraft.EntityFrameworkCore.Entities;

namespace SkillCraft.Application.Worlds.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class CreateWorldCommandTests : IntegrationTests
{
  private readonly IWorldRepository _worldRepository;

  public CreateWorldCommandTests() : base()
  {
    _worldRepository = ServiceProvider.GetRequiredService<IWorldRepository>();
  }

  [Fact(DisplayName = "It should create a new world.")]
  public async Task It_should_create_a_new_world()
  {
    CreateWorldPayload payload = new("universe")
    {
      DisplayName = " Universe ",
      Description = "    "
    };
    CreateWorldCommand command = new(payload);
    World world = await Pipeline.ExecuteAsync(command);

    Assert.Equal(2, world.Version);
    Assert.Equal(Actor, world.CreatedBy);
    Assert.Equal(Actor, world.UpdatedBy);
    Assert.True(world.CreatedOn < world.UpdatedOn);

    Assert.Equal(payload.UniqueSlug.Trim(), world.UniqueSlug);
    Assert.Equal(payload.DisplayName.CleanTrim(), world.DisplayName);
    Assert.Equal(payload.Description.CleanTrim(), world.Description);

    WorldEntity? entity = await SkillCraftContext.Worlds.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == new AggregateId(world.Id).Value);
    Assert.NotNull(entity);
  }

  [Fact(DisplayName = "It should throw PermissionDeniedException when the permission is denied.")]
  public async Task It_should_throw_PermissionDeniedException_when_the_permission_is_denied()
  {
    WorldAggregate world = new(new UniqueSlugUnit("old-universe"), ActorId);
    await _worldRepository.SaveAsync(world);

    CreateWorldPayload payload = new("new-universe");
    CreateWorldCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<PermissionDeniedException>(async () => await Pipeline.ExecuteAsync(command, CancellationToken));
    Assert.Equal(Actor.ToString(), exception.Actor);
    Assert.Equal("CreateWorld", exception.Permission);
  }

  [Fact(DisplayName = "It should throw UniqueSlugAlreadyUsedException when the unique slug is already used.")]
  public async Task It_should_throw_UniqueSlugAlreadyUsedException_when_the_unique_slug_is_already_used()
  {
    WorldAggregate world = new(new UniqueSlugUnit("universe"));
    await _worldRepository.SaveAsync(world);

    CreateWorldPayload payload = new(world.UniqueSlug.Value);
    CreateWorldCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<UniqueSlugAlreadyUsedException<WorldAggregate>>(async () => await Pipeline.ExecuteAsync(command, CancellationToken));
    Assert.Equal(world.UniqueSlug, exception.UniqueSlug);
    Assert.Equal("UniqueSlug", exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateWorldPayload payload = new("hello-world-!");
    CreateWorldCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command, CancellationToken));

    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal("SlugValidator", error.ErrorCode);
    Assert.Equal("UniqueSlug", error.PropertyName);
  }
}
