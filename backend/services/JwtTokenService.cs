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
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("Jwt:Audience is missing.");
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("Jwt:Secret is missing.");
        var expiresMinutesRaw = Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES") ?? "60";

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
}
