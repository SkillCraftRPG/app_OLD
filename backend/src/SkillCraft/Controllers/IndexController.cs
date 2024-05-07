using Microsoft.AspNetCore.Mvc;
using SkillCraft.Models.Index;

namespace SkillCraft.Controllers;

[ApiController]
[Route("")]
public class IndexController : ControllerBase
{
  [HttpGet]
  public ActionResult<ApiVersion> Get() => Ok(ApiVersion.Current);
}
