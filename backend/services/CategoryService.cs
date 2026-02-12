using Microsoft.EntityFrameworkCore;
using backend.Errors;
using backend.Database;
using backend.Models;

namespace backend.Services;

public sealed class CategoryService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwtTokenService;

    public CategoryService(AppDbContext db, JwtTokenService jwtTokenService)
    {
        _db = db;
        _jwtTokenService = jwtTokenService;
    }

    public Task<List<Categories>> GetAllAsync()
    {
        return _db.Categories.AsNoTracking().ToListAsync();
    }
}