﻿namespace SkillCraft.Contracts.Accounts;

public record SaveProfilePayload
{
  public string FirstName { get; set; }
  public string? MiddleName { get; set; }
  public string LastName { get; set; }

  public DateTime? Birthdate { get; set; }
  public string? Gender { get; set; }
  public string Locale { get; set; }
  public string TimeZone { get; set; }

  public SaveProfilePayload() : this(string.Empty, string.Empty, string.Empty, string.Empty)
  {
  }

  public SaveProfilePayload(string firstName, string lastName, string locale, string timeZone)
  {
    FirstName = firstName;
    LastName = lastName;

    Locale = locale;
    TimeZone = timeZone;
  }
}
