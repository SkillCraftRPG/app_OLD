using Bogus;
using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Realms;
using Logitar.Portal.Contracts.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SkillCraft.Application;
using SkillCraft.Application.Accounts;
using SkillCraft.EntityFrameworkCore;
using SkillCraft.EntityFrameworkCore.Entities;
using SkillCraft.EntityFrameworkCore.PostgreSQL;
using SkillCraft.EntityFrameworkCore.SqlServer;
using SkillCraft.Infrastructure;
using SkillCraft.Infrastructure.Commands;

namespace SkillCraft;

public abstract class IntegrationTests : IAsyncLifetime
{
  private readonly TestContext _context = new();
  private readonly DatabaseProvider _databaseProvider;

  protected CancellationToken CancellationToken { get; }
  protected Faker Faker { get; } = new();

  protected IConfiguration Configuration { get; }
  protected IServiceProvider ServiceProvider { get; }

  protected EventContext EventContext { get; }
  protected SkillCraftContext SkillCraftContext { get; }

  protected IRequestPipeline Pipeline { get; }

  protected Mock<IApiKeyService> ApiKeyService { get; } = new();
  protected Mock<IMessageService> MessageService { get; } = new();
  protected Mock<IOneTimePasswordService> OneTimePasswordService { get; } = new();
  protected Mock<IRealmService> RealmService { get; } = new();
  protected Mock<ISessionService> SessionService { get; } = new();
  protected Mock<ITokenService> TokenService { get; } = new();
  protected Mock<IUserService> UserService { get; } = new();

  protected Actor Actor { get; } = Actor.System;
  protected ActorId ActorId => new(Actor.Id);

  protected IntegrationTests()
  {
    Configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    ServiceCollection services = new();
    services.AddSingleton(Configuration);
    services.AddSingleton(_context);
    services.AddSingleton<IRequestPipeline, TestRequestPipeline>();

    string connectionString;
    _databaseProvider = Configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (_databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        connectionString = Configuration.GetValue<string>("POSTGRESQLCONNSTR_SkillCraft")?.Replace("{Database}", GetType().Name) ?? string.Empty;
        services.AddSkillCraftWithEntityFrameworkCorePostgreSQL(connectionString);
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        connectionString = Configuration.GetValue<string>("SQLCONNSTR_SkillCraft")?.Replace("{Database}", GetType().Name) ?? string.Empty;
        services.AddSkillCraftWithEntityFrameworkCoreSqlServer(connectionString);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(_databaseProvider);
    }

    services.AddSingleton(ApiKeyService.Object);
    services.AddSingleton(MessageService.Object);
    services.AddSingleton(OneTimePasswordService.Object);
    services.AddSingleton(RealmService.Object);
    services.AddSingleton(SessionService.Object);
    services.AddSingleton(TokenService.Object);
    services.AddSingleton(UserService.Object);

    ServiceProvider = services.BuildServiceProvider();

    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    SkillCraftContext = ServiceProvider.GetRequiredService<SkillCraftContext>();

    Pipeline = ServiceProvider.GetRequiredService<IRequestPipeline>();

    Realm realm = new("skillcraftrpg", "cz[ySe2#xrq-aKWFQP$m58@U9&4Xk'tJ")
    {
      DisplayName = "SkillCraftRPG",
      DefaultLocale = new Locale("fr"),
      Url = "https://app.skillcraftrpg.ca/"
    };
    RealmService.Setup(x => x.FindAsync(CancellationToken)).ReturnsAsync(realm);

    DateTime now = DateTime.Now;
    User user = new(Faker.Person.UserName)
    {
      Id = Guid.NewGuid(),
      Version = 1,
      CreatedOn = now,
      UpdatedOn = now,
      Email = new Email(Faker.Person.Email),
      FirstName = Faker.Person.FirstName,
      LastName = Faker.Person.LastName,
      FullName = Faker.Person.FullName,
      Birthdate = Faker.Person.DateOfBirth,
      Gender = Faker.Person.Gender.ToString().ToLower(),
      Picture = Faker.Person.Avatar,
      Website = $"https://www.{Faker.Person.Website}",
      Realm = realm
    };
    Actor = new(user);
    user.CreatedBy = Actor;
    user.UpdatedBy = Actor;
    _context.User = user;
  }

  public virtual async Task InitializeAsync()
  {
    IPublisher publisher = ServiceProvider.GetRequiredService<IPublisher>();
    await publisher.Publish(new InitializeDatabaseCommand());

    StringBuilder command = new();
    command.AppendLine(CreateDeleteBuilder(SkillCraftDb.Worlds.Table).Build().Text);
    command.AppendLine(CreateDeleteBuilder(SkillCraftDb.Actors.Table).Build().Text);
    command.AppendLine(CreateDeleteBuilder(EventDb.Events.Table).Build().Text);
    await SkillCraftContext.Database.ExecuteSqlRawAsync(command.ToString());

    if (_context.User != null)
    {
      ActorEntity actor = new(_context.User);
      SkillCraftContext.Actors.Add(actor);
      await SkillCraftContext.SaveChangesAsync();
    }
  }
  private IDeleteBuilder CreateDeleteBuilder(TableId table) => _databaseProvider switch
  {
    DatabaseProvider.EntityFrameworkCorePostgreSQL => PostgresDeleteBuilder.From(table),
    DatabaseProvider.EntityFrameworkCoreSqlServer => SqlServerDeleteBuilder.From(table),
    _ => throw new DatabaseProviderNotSupportedException(_databaseProvider),
  };

  public virtual Task DisposeAsync() => Task.CompletedTask;
}
