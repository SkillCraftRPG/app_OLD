﻿using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Passwords;
using Logitar.Portal.Contracts.Users;
using SkillCraft.Application.Accounts;
using SkillCraft.Contracts.Accounts;

namespace SkillCraft.Infrastructure.IdentityServices;

internal class OneTimePasswordService : IOneTimePasswordService
{
  private const string Characters = "0123456789";
  private const int Length = 6;
  private const int LifetimeSeconds = 60 * 60;
  private const int MaximumAttempts = 5;

  private readonly IOneTimePasswordClient _oneTimePasswordClient;

  public OneTimePasswordService(IOneTimePasswordClient oneTimePasswordClient)
  {
    _oneTimePasswordClient = oneTimePasswordClient;
  }

  public async Task<OneTimePassword> CreateAsync(User user, string purpose, CancellationToken cancellationToken)
  {
    return await CreateAsync(user, purpose, phone: null, cancellationToken);
  }
  public async Task<OneTimePassword> CreateAsync(User user, string purpose, Phone? phone, CancellationToken cancellationToken)
  {
    CreateOneTimePasswordPayload payload = new(Characters, Length)
    {
      ExpiresOn = DateTime.Now.AddSeconds(LifetimeSeconds),
      MaximumAttempts = MaximumAttempts
    };
    payload.SetUserId(user);
    payload.SetPurpose(purpose);
    if (phone != null)
    {
      payload.SetPhone(phone);
    }
    RequestContext context = new(user.Id.ToString(), cancellationToken);
    return await _oneTimePasswordClient.CreateAsync(payload, context);
  }

  public async Task<OneTimePassword> ValidateAsync(OneTimePasswordPayload oneTimePasswordPayload, string purpose, CancellationToken cancellationToken)
  {
    ValidateOneTimePasswordPayload payload = new(oneTimePasswordPayload.Code);
    RequestContext context = new(cancellationToken);
    OneTimePassword oneTimePassword = await _oneTimePasswordClient.ValidateAsync(oneTimePasswordPayload.Id, payload, context)
      ?? throw new OneTimePasswordNotFoundException(oneTimePasswordPayload.Id);
    oneTimePassword.EnsurePurpose(purpose);
    return oneTimePassword;
  }
}
