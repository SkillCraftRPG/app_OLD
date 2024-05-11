using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Contracts.Worlds;
using SkillCraft.Domain.Shared;
using SkillCraft.Domain.Worlds;

namespace SkillCraft.Application.Worlds.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class ReadWorldQueryTests : IntegrationTests
{
  private readonly IWorldRepository _worldRepository;

  private readonly WorldAggregate _alternate;
  private readonly WorldAggregate _universe;

  public ReadWorldQueryTests() : base()
  {
    _worldRepository = ServiceProvider.GetRequiredService<IWorldRepository>();

    _alternate = new WorldAggregate(new UniqueSlugUnit("alternate"), ActorId);
    _universe = new WorldAggregate(new UniqueSlugUnit("universe"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _worldRepository.SaveAsync([_alternate, _universe]);
  }

  [Fact(DisplayName = "It should return null when no world matches.")]
  public async Task It_should_return_null_when_no_world_matches()
  {
    ReadWorldQuery query = new(Id: Guid.Empty, UniqueSlug: "test");
    World? world = await Pipeline.ExecuteAsync(query);
    Assert.Null(world);
  }

  [Fact(DisplayName = "It should return the world found by ID.")]
  public async Task It_should_return_the_world_found_by_Id()
  {
    ReadWorldQuery query = new(_universe.Id.ToGuid(), UniqueSlug: null);
    World? world = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(world);
    Assert.Equal(_universe.Id.ToGuid(), world.Id);
  }

  [Fact(DisplayName = "It should return the world found by unique slug.")]
  public async Task It_should_return_the_world_found_by_unique_slug()
  {
    ReadWorldQuery query = new(Id: null, _alternate.UniqueSlug.Value);
    World? world = await Pipeline.ExecuteAsync(query);
    Assert.NotNull(world);
    Assert.Equal(_alternate.Id.ToGuid(), world.Id);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when many worlds match.")]
  public async Task It_should_throw_TooManyResultsException_when_many_worlds_match()
  {
    ReadWorldQuery query = new(_universe.Id.ToGuid(), _alternate.UniqueSlug.Value);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<World>>(async () => await Pipeline.ExecuteAsync(query));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }

  // TODO(fpion): try reading a world you cannot read
}
