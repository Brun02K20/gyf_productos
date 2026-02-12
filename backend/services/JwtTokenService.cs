using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        DotNetEnv.Env.Load();
        _configuration = configuration;
    }

    public (string Token, DateTime ExpiresAtUtc) CreateToken(Users user)
{
    var issuer = GetRequiredValue("JWT_ISSUER", "Jwt:Issuer is missing.");
    var audience = GetRequiredValue("JWT_AUDIENCE", "Jwt:Audience is missing.");
    var secret = GetRequiredValue("JWT_SECRET", "Jwt:Secret is missing.");
    var expiresMinutesRaw = GetOptionalValue("JWT_EXPIRATION_MINUTES") ?? "60";

    if (!int.TryParse(expiresMinutesRaw, out var expiresMinutes))
    {
        expiresMinutes = 60;
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    };

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: expiresAtUtc,
        signingCredentials: credentials);

    var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
    return (tokenValue, expiresAtUtc);
}

private string GetRequiredValue(string key, string error)
{
    var value = _configuration[key];
    if (string.IsNullOrWhiteSpace(value))
    {
        value = Environment.GetEnvironmentVariable(key);
    }

    if (string.IsNullOrWhiteSpace(value))
    {
        throw new InvalidOperationException(error);
    }

    return value;
}

private string? GetOptionalValue(string key)
{
    return _configuration[key] ?? Environment.GetEnvironmentVariable(key);
}
}
