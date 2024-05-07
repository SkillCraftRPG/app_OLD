﻿using Bogus;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Passwords;
using Logitar.Portal.Contracts.Users;

namespace SkillCraft.Application.Accounts;

[Trait(Traits.Category, Categories.Unit)]
public class OneTimePasswordExtensionsTests
{
  private const string Purpose = "MultiFactorAuthentication";

  private readonly Faker _faker = new();

  [Fact(DisplayName = "EnsurePurpose: it should do nothing when the One-Time Password has the expected purpose.")]
  public void EnsurePurpose_it_should_do_nothing_when_the_One_Time_Password_has_the_expected_purpose()
  {
    OneTimePassword password = new();
    password.CustomAttributes.Add(new("Purpose", Purpose));
    password.EnsurePurpose(Purpose);
  }

  [Fact(DisplayName = "EnsurePurpose: it should throw InvalidOneTimePasswordPurpose when the One-Time Password has no purpose.")]
  public void EnsurePurpose_it_should_throw_InvalidOneTimePasswordPurpose_when_the_One_Time_Password_has_no_purpose()
  {
    OneTimePassword password = new()
    {
      Id = Guid.NewGuid()
    };
    Assert.Empty(password.CustomAttributes);
    var exception = Assert.Throws<InvalidOneTimePasswordPurposeException>(() => password.EnsurePurpose(Purpose));
    Assert.Equal(password.Id, exception.OneTimePasswordId);
    Assert.Equal(Purpose, exception.ExpectedPurpose);
    Assert.Null(exception.ActualPurpose);
  }

  [Fact(DisplayName = "EnsurePurpose: it should throw InvalidOneTimePasswordPurpose when the purpose is not expected.")]
  public void EnsurePurpose_it_should_throw_InvalidOneTimePasswordPurpose_when_the_purpose_is_not_expected()
  {
    OneTimePassword password = new()
    {
      Id = Guid.NewGuid()
    };

    string purpose = "Test";
    password.CustomAttributes.Add(new("Purpose", purpose));

    var exception = Assert.Throws<InvalidOneTimePasswordPurposeException>(() => password.EnsurePurpose(Purpose));
    Assert.Equal(password.Id, exception.OneTimePasswordId);
    Assert.Equal(Purpose, exception.ExpectedPurpose);
    Assert.Equal(purpose, exception.ActualPurpose);
  }

  [Fact(DisplayName = "GetPurpose: it should return the purpose when the One-Time Password has one.")]
  public void GetPurpose_it_should_return_the_purpose_when_the_One_Time_Password_has_one()
  {
    OneTimePassword password = new();
    password.CustomAttributes.Add(new("Purpose", Purpose));
    Assert.Equal(Purpose, password.GetPurpose());
  }

  [Fact(DisplayName = "GetPurpose: it should throw ArgumentException when the One-Time Password has no purpose.")]
  public void GetPurpose_it_should_throw_ArgumentException_when_the_One_Time_Password_has_no_purpose()
  {
    OneTimePassword password = new();
    Assert.Empty(password.CustomAttributes);
    var exception = Assert.Throws<ArgumentException>(password.GetPurpose);
    Assert.StartsWith("The One-Time Password (OTP) has no 'Purpose' custom attribute.", exception.Message);
    Assert.Equal("oneTimePassword", exception.ParamName);
  }

  [Fact(DisplayName = "GetUserId: it should return the correct identifier.")]
  public void GetUserId_it_should_return_the_correct_identifier()
  {
    Guid userId = Guid.NewGuid();
    OneTimePassword password = new();
    password.CustomAttributes.Add(new("UserId", userId.ToString()));
    Assert.Equal(userId, password.GetUserId());
  }

  [Fact(DisplayName = "GetUserId: it should throw ArgumentException when the One-Time Password does not have the custom attribute.")]
  public void GetUserId_it_should_throw_ArgumentException_when_the_One_Time_Password_does_not_have_the_custom_attribute()
  {
    Guid userId = Guid.NewGuid();
    OneTimePassword password = new();
    Assert.Empty(password.CustomAttributes);

    var exception = Assert.Throws<ArgumentException>(() => password.GetUserId());
    Assert.StartsWith("The One-Time Password (OTP) has no 'UserId' custom attribute.", exception.Message);
    Assert.Equal("oneTimePassword", exception.ParamName);
  }

  [Fact(DisplayName = "GetPhone: it should return the correct phone.")]
  public void GetPhone_it_should_return_the_correct_phone()
  {
    OneTimePassword oneTimePassword = new();
    oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneCountryCode", "CA"));
    oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneNumber", "(514) 845-4636"));
    oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneE164Formatted", "+15148454636"));

    Phone phone = oneTimePassword.GetPhone();
    Assert.Equal("CA", phone.CountryCode);
    Assert.Equal("(514) 845-4636", phone.Number);
    Assert.Null(phone.Extension);
    Assert.Equal("+15148454636", phone.E164Formatted);
    Assert.False(phone.IsVerified);
    Assert.Null(phone.VerifiedBy);
    Assert.Null(phone.VerifiedOn);
  }

  [Fact(DisplayName = "GetPhone: it should throw ArgumentException when the One-Time Password does not have the custom attributes.")]
  public void GetPhone_it_should_throw_ArgumentException_when_the_One_Time_Password_does_not_have_the_custom_attributes()
  {
    OneTimePassword oneTimePassword = new();
    var exception = Assert.Throws<ArgumentException>(() => oneTimePassword.GetPhone());
    Assert.StartsWith("The One-Time Password (OTP) does not have phone custom attributes.", exception.Message);
    Assert.Equal("oneTimePassword", exception.ParamName);
  }

  [Fact(DisplayName = "HasPurpose: it should return false when the One-Time Password does not have the expected purpose.")]
  public void HasPurpose_it_should_return_false_when_the_One_Time_Password_does_not_have_the_expected_purpose()
  {
    OneTimePassword password = new();
    Assert.Empty(password.CustomAttributes);
    Assert.False(password.HasPurpose(Purpose));

    password.CustomAttributes.Add(new("Purpose", "Test"));
    Assert.False(password.HasPurpose(Purpose));
  }

  [Fact(DisplayName = "HasPurpose: it should return true when the One-Time Password has the expected purpose.")]
  public void HasPurpose_it_should_return_true_when_the_One_Time_Password_has_the_expected_purpose()
  {
    OneTimePassword password = new();
    password.CustomAttributes.Add(new("Purpose", Purpose.ToUpper()));
    Assert.True(password.HasPurpose(Purpose.ToLower()));
  }

  [Theory(DisplayName = "SetPhone: it should set the correct custom attributes.")]
  [InlineData(null, "(514) 845-4636", "+15148454636")]
  [InlineData("CA", "(514) 845-4636", "+15148454636")]
  public void SetPhone_it_should_set_the_correct_custom_attributes(string? countryCode, string number, string e164Formatted)
  {
    Phone phone = new(countryCode, number, extension: null, e164Formatted);
    CreateOneTimePasswordPayload payload = new("0123456789", length: 6);
    payload.SetPhone(phone);

    Assert.Contains(payload.CustomAttributes, c => c.Key == "PhoneNumber" && c.Value == number);
    Assert.Contains(payload.CustomAttributes, c => c.Key == "PhoneE164Formatted" && c.Value == e164Formatted);

    if (countryCode == null)
    {
      Assert.Equal(2, payload.CustomAttributes.Count);
    }
    else
    {
      Assert.Equal(3, payload.CustomAttributes.Count);
      Assert.Contains(payload.CustomAttributes, c => c.Key == "PhoneCountryCode" && c.Value == countryCode);
    }
  }

  [Fact(DisplayName = "SetPurpose: it should add the correct custom attribute.")]
  public void SetPurpose_it_should_add_the_correct_custom_attribute()
  {
    CreateOneTimePasswordPayload payload = new();
    Assert.Empty(payload.CustomAttributes);

    payload.SetPurpose(Purpose);
    Assert.Contains(payload.CustomAttributes, c => c.Key == "Purpose" && c.Value == Purpose);
  }

  [Fact(DisplayName = "SetPurpose: it should replace the correct custom attribute.")]
  public void SetPurpose_it_should_replace_the_correct_custom_attribute()
  {
    CreateOneTimePasswordPayload payload = new();
    payload.CustomAttributes.Add(new("UserId", Guid.NewGuid().ToString()));

    payload.SetPurpose(Purpose);
    Assert.Single(payload.CustomAttributes, c => c.Key == "Purpose" && c.Value == Purpose);
  }

  [Fact(DisplayName = "SetUserId: it should add the correct custom attribute.")]
  public void SetUserId_it_should_add_the_correct_custom_attribute()
  {
    CreateOneTimePasswordPayload payload = new();
    Assert.Empty(payload.CustomAttributes);

    User user = new(_faker.Person.UserName)
    {
      Id = Guid.NewGuid()
    };
    payload.SetUserId(user);
    Assert.Contains(payload.CustomAttributes, c => c.Key == "UserId" && c.Value == user.Id.ToString());
  }

  [Fact(DisplayName = "SetUserId: it should replace the correct custom attribute.")]
  public void SetUserId_it_should_replace_the_correct_custom_attribute()
  {
    CreateOneTimePasswordPayload payload = new();
    payload.CustomAttributes.Add(new("UserId", Guid.NewGuid().ToString()));

    User user = new(_faker.Person.UserName)
    {
      Id = Guid.NewGuid()
    };
    payload.SetUserId(user);
    Assert.Single(payload.CustomAttributes, c => c.Key == "UserId" && c.Value == user.Id.ToString());
  }

  [Theory(DisplayName = "TryGetPhone: it should return null when the One-Time Password does not have the custom attributes.")]
  [InlineData(null, null)]
  [InlineData("(514) 845-4636", null)]
  [InlineData(null, "+15148454636")]
  public void TryGetPhone_it_should_return_null_when_the_One_Time_Password_does_not_have_the_custom_attributes(string? number, string? e164Formatted)
  {
    Assert.True(number == null || e164Formatted == null);

    OneTimePassword oneTimePassword = new();
    if (number != null)
    {
      oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneNumber", number));
    }
    if (e164Formatted != null)
    {
      oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneE164Formatted", e164Formatted));
    }

    Assert.Null(oneTimePassword.TryGetPhone());
  }

  [Theory(DisplayName = "TryGetPhone: it should return the correct phone.")]
  [InlineData(null, "(514) 845-4636", "+15148454636")]
  [InlineData("CA", "(514) 845-4636", "+15148454636")]
  public void TryGetPhone_it_should_return_the_correct_phone(string? countryCode, string number, string e164Formatted)
  {
    OneTimePassword oneTimePassword = new();
    if (countryCode != null)
    {
      oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneCountryCode", countryCode));
    }
    oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneNumber", number));
    oneTimePassword.CustomAttributes.Add(new CustomAttribute("PhoneE164Formatted", e164Formatted));

    Phone? phone = oneTimePassword.TryGetPhone();
    Assert.NotNull(phone);
    Assert.Equal(countryCode, phone.CountryCode);
    Assert.Equal(number, phone.Number);
    Assert.Null(phone.Extension);
    Assert.Equal(e164Formatted, phone.E164Formatted);
    Assert.False(phone.IsVerified);
    Assert.Null(phone.VerifiedBy);
    Assert.Null(phone.VerifiedOn);
  }

  [Fact(DisplayName = "TryGetPurpose: it should return null when the One-Time Password has no purpose.")]
  public void TryGetPurpose_it_should_return_null_when_the_One_Time_Password_has_no_purpose()
  {
    OneTimePassword password = new();
    Assert.Empty(password.CustomAttributes);
    Assert.Null(password.TryGetPurpose());
  }

  [Fact(DisplayName = "TryGetPurpose: it should return the purpose when the One-Time Password has one.")]
  public void TryGetPurpose_it_should_return_the_purpose_when_the_One_Time_Password_has_one()
  {
    OneTimePassword password = new();
    password.CustomAttributes.Add(new("Purpose", Purpose));
    Assert.Equal(Purpose, password.TryGetPurpose());
  }

  [Fact(DisplayName = "TryGetUserId: it should return null when the One-Time Password does not have the custom attribute.")]
  public void TryGetUserId_it_should_return_null_when_the_One_Time_Password_does_not_have_the_custom_attribute()
  {
    OneTimePassword oneTimePassword = new();
    Assert.Null(oneTimePassword.TryGetUserId());
  }

  [Fact(DisplayName = "TryGetUserId: it should return the user Id when the One-Time Password has one.")]
  public void TryGetUserId_it_should_return_the_user_Id_when_the_One_Time_Password_has_one()
  {
    Guid userId = Guid.NewGuid();

    OneTimePassword oneTimePassword = new();
    oneTimePassword.CustomAttributes.Add(new CustomAttribute("UserId", userId.ToString()));

    Assert.Equal(userId, oneTimePassword.TryGetUserId());
  }
}
