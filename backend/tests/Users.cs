using backend.Controllers;
using backend.Database;
using backend.Dtos.Users;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.Configuration;
using backend.Errors;

namespace backend.Tests;

public sealed class UserControllerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JWT_ISSUER"] = "test-issuer",
                ["JWT_AUDIENCE"] = "test-audience",
                ["JWT_SECRET"] = "test-secret-1234567890-1234567890-1234",
                ["JWT_EXPIRATION_MINUTES"] = "60"
            })
            .Build();

        var tokenService = new JwtTokenService(config);
        var service = new UserService(_db, tokenService);
        _controller = new UserController(service);
    }

    [Fact]
    public async Task Register_creates_user()
    {
        var request = new UserCredentials
        {
            Username = "ana",
            Password = "secret"
        };

        var result = await _controller.Register(request);

        var created = Assert.IsType<CreatedResult>(result.Result);
        var payload = Assert.IsType<RegisterUser>(created.Value);

        Assert.True(payload.Id > 0);
        Assert.Equal("ana", payload.Username);
        Assert.False(string.IsNullOrWhiteSpace(payload.Password));

        var dbUser = await _db.Users.SingleAsync();
        Assert.True(BCrypt.Net.BCrypt.Verify("secret", dbUser.PasswordHash));
    }

    [Fact]
    public async Task Login_returns_token()
    {
        SeedUser("ana", "secret");

        var result = await _controller.Login(new UserCredentials
        {
            Username = "ana",
            Password = "secret"
        });

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<LoginUser>(ok.Value);

        Assert.Equal("ana", payload.Username);
        Assert.False(string.IsNullOrWhiteSpace(payload.Token));
    }

    [Fact]
    public async Task Login_invalid_password_throws_unauthorized()
    {
        SeedUser("ana", "secret");

        await Assert.ThrowsAsync<UnauthorizedError>(() =>
            _controller.Login(new UserCredentials { Username = "ana", Password = "wrong" }));
    }

    [Fact]
    public async Task Register_duplicate_user_throws_conflict()
    {
        SeedUser("ana", "secret");

        await Assert.ThrowsAsync<ConflictError>(() =>
            _controller.Register(new UserCredentials { Username = "ana", Password = "secret" }));
    }

    [Fact]
    public async Task Register_missing_fields_throws_invalid_params()
    {
        await Assert.ThrowsAsync<InvalidParamsError>(() =>
            _controller.Register(new UserCredentials { Username = "", Password = "" }));
    }

    private void SeedUser(string username, string password)
    {
        _db.Users.Add(new Users
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        });
        _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    
}