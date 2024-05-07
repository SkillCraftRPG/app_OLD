﻿using Logitar.Portal.Contracts.Messages;
using Logitar.Portal.Contracts.Users;

namespace SkillCraft.Application.Accounts;

public interface IMessageService
{
  Task<SentMessages> SendAsync(string template, Email email, string? locale = null, Dictionary<string, string>? variables = null, CancellationToken cancellationToken = default);
  Task<SentMessages> SendAsync(string template, Phone phone, string? locale = null, Dictionary<string, string>? variables = null, CancellationToken cancellationToken = default);
  Task<SentMessages> SendAsync(string template, User user, string? locale = null, Dictionary<string, string>? variables = null, CancellationToken cancellationToken = default);
}
