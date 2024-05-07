﻿using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Application;
using SkillCraft.Application.Accounts;
using SkillCraft.Application.Accounts.Commands;
using SkillCraft.Authentication;
using SkillCraft.Contracts.Accounts;
using SkillCraft.Extensions;
using SkillCraft.Models.Account;

namespace SkillCraft.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
  private readonly IBearerTokenService _bearerTokenService;
  private readonly IRequestPipeline _requestPipeline;
  private readonly ISessionService _sessionService;

  public AccountController(IBearerTokenService bearerTokenService, IRequestPipeline requestPipeline, ISessionService sessionService)
  {
    _bearerTokenService = bearerTokenService;
    _requestPipeline = requestPipeline;
    _sessionService = sessionService;
  }

  [HttpPost("/auth/sign/in")]
  public async Task<ActionResult<SignInResponse>> SignInAsync([FromBody] SignInPayload payload, CancellationToken cancellationToken)
  {
    SignInCommandResult result = await _requestPipeline.ExecuteAsync(new SignInCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
    if (result.Session != null)
    {
      HttpContext.SignIn(result.Session);
    }

    return Ok(new SignInResponse(result));
  }

  [HttpPost("/auth/sign/out")]
  [Authorize]
  public async Task<ActionResult> SignOutAsync(bool everywhere, CancellationToken cancellationToken)
  {
    Session? session = HttpContext.GetSession();
    User? user = HttpContext.GetUser();
    if (everywhere && user != null)
    {
      SignOutCommand command = new(user);
      await _requestPipeline.ExecuteAsync(command, cancellationToken);
    }
    else if (!everywhere && session != null)
    {
      SignOutCommand command = new(session);
      await _requestPipeline.ExecuteAsync(command, cancellationToken);
    }

    HttpContext.SignOut();

    return NoContent();
  }

  [HttpPost("/auth/token")]
  public async Task<ActionResult<GetTokenResponse>> TokenAsync([FromBody] GetTokenPayload payload, CancellationToken cancellationToken)
  {
    GetTokenResponse response;
    Session? session;
    if (!string.IsNullOrWhiteSpace(payload.RefreshToken))
    {
      response = new GetTokenResponse();
      session = await _sessionService.RenewAsync(payload.RefreshToken, HttpContext.GetSessionCustomAttributes(), cancellationToken);
    }
    else
    {
      SignInCommandResult result = await _requestPipeline.ExecuteAsync(new SignInCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
      response = new(result);
      session = result.Session;
    }

    if (session != null)
    {
      response.TokenResponse = _bearerTokenService.GetTokenResponse(session);
    }

    return Ok(response);
  }

  [HttpPost("password/reset")]
  public async Task<ActionResult<ResetPasswordResponse>> ResetPasswordAsync([FromBody] ResetPasswordPayload payload, CancellationToken cancellationToken)
  {
    ResetPasswordResult result = await _requestPipeline.ExecuteAsync(new ResetPasswordCommand(payload, HttpContext.GetSessionCustomAttributes()), cancellationToken);
    if (result.Session != null)
    {
      HttpContext.SignIn(result.Session);
    }

    return Ok(new ResetPasswordResponse(result));
  }

  [HttpPut("/phone/change")]
  [Authorize]
  public async Task<ActionResult<ChangePhoneResult>> ChangePhoneAsync([FromBody] ChangePhonePayload payload, CancellationToken cancellationToken)
  {
    ChangePhoneResult result = await _requestPipeline.ExecuteAsync(new ChangePhoneCommand(payload), cancellationToken);
    return Ok(result);
  }

  [HttpPost("/phone/verify")]
  public async Task<ActionResult<VerifyPhoneResult>> VerifyPhoneAsync([FromBody] VerifyPhonePayload payload, CancellationToken cancellationToken)
  {
    VerifyPhoneResult result = await _requestPipeline.ExecuteAsync(new VerifyPhoneCommand(payload), cancellationToken);
    return Ok(result);
  }

  [HttpGet("/profile")]
  [Authorize]
  public ActionResult<UserProfile> GetProfile()
  {
    User user = HttpContext.GetUser() ?? throw new InvalidOperationException("An authenticated user is required.");
    return Ok(user.ToUserProfile());
  }
}
