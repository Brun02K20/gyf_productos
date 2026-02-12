using Microsoft.EntityFrameworkCore;
using backend.Database;
using backend.Models;
using backend.Dtos.Users;
using backend.Errors;

namespace backend.Services;

public sealed class UserService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwtTokenService;

    public UserService(AppDbContext db, JwtTokenService jwtTokenService)
    {
        _db = db;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginUser> LoginAsync(UserCredentials credentials)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(credentials.Username) || string.IsNullOrWhiteSpace(credentials.Password))
            {
                throw new InvalidParamsError("Username and password are required.");
            }

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == credentials.Username);
            if (user is null)
            {
                throw new UnauthorizedError("Invalid credentials.");
            }

            var ok = BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash);
            if (!ok)
            {
                throw new UnauthorizedError("Invalid credentials.");
            }

            var (token, expiresAtUtc) = _jwtTokenService.CreateToken(user);

            return new LoginUser
            {
                Id = user.Id,
                Token = token,
                Username = user.Username
            };
        }
        catch (HTTPError)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InternalServerError();
        }
    }

    public async Task<RegisterUser> RegisterAsync(UserCredentials request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new InvalidParamsError("Username and password are required.");
            }

            var existing = await _db.Users.AnyAsync(user => user.Username == request.Username);
            if (existing)
            {
                throw new ConflictError("User already exists.");
            }

            var user = new Users
            {
                Username = request.Username.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new RegisterUser
            {
                Id = user.Id,
                Username = user.Username,
                Password =  user.PasswordHash
            };
        }
        catch (HTTPError)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InternalServerError();
        }
    }
}