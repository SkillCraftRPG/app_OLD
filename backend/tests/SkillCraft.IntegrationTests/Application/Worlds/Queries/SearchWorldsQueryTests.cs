using SkillCraft.Contracts.Search;
using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Application.Worlds.Queries;

[Trait(Traits.Category, Categories.Integration)]
public class SearchWorldsQueryTests : IntegrationTests
{
  public SearchWorldsQueryTests() : base()
  {
  }

  [Fact(DisplayName = "It should return empty results when no world match.")]
  public async Task It_should_return_empty_results_when_no_world_match()
  {
    SearchWorldsPayload payload = new()
    {
      Search = new TextSearch([new SearchTerm("%test%")])
    };
    SearchWorldsQuery query = new(payload);
    SearchResults<World> results = await Pipeline.ExecuteAsync(query);
    Assert.Empty(results.Items);
    Assert.Equal(0, results.Total);
  }

  // TODO(fpion): complete
}
