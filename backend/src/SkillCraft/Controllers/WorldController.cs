using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Application;
using SkillCraft.Application.Worlds.Commands;
using SkillCraft.Application.Worlds.Queries;
using SkillCraft.Contracts.Search;
using SkillCraft.Contracts.Worlds;
using SkillCraft.Extensions;
using SkillCraft.Models.Worlds;

namespace SkillCraft.Controllers;

[ApiController]
[Authorize]
[Route("worlds")]
public class WorldController : ControllerBase
{
  private readonly IRequestPipeline _requestPipeline;

  public WorldController(IRequestPipeline requestPipeline)
  {
    _requestPipeline = requestPipeline;
  }

  [HttpPost]
  public async Task<ActionResult<World>> CreateAsync([FromBody] CreateWorldPayload payload, CancellationToken cancellationToken)
  {
    World world = await _requestPipeline.ExecuteAsync(new CreateWorldCommand(payload), cancellationToken);
    return Created(BuildLocation(world), world);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<World>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    World? world = await _requestPipeline.ExecuteAsync(new ReadWorldQuery(id, UniqueSlug: null), cancellationToken);
    return world == null ? NotFound() : Ok(world);
  }

  [HttpGet("unique-slug:{uniqueSlug}")]
  public async Task<ActionResult<World>> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    World? world = await _requestPipeline.ExecuteAsync(new ReadWorldQuery(Id: null, uniqueSlug), cancellationToken);
    return world == null ? NotFound() : Ok(world);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<World>>> SearchAsync([FromQuery] SearchWorldsModel model, CancellationToken cancellationToken)
  {
    SearchResults<World> worlds = await _requestPipeline.ExecuteAsync(new SearchWorldsQuery(model.ToPayload()), cancellationToken);
    return Ok(worlds);
  }

  private Uri BuildLocation(World world) => HttpContext.BuildLocation("worlds/{id}", new Dictionary<string, string> { ["id"] = world.Id.ToString() });
}
