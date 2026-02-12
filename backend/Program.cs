using backend.Database;
using backend.Middleware;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using backend.Models;



var builder = WebApplication.CreateBuilder(args);

// Cargo las variables de entorno desde el archivo .env
DotNetEnv.Env.Load();

// Agrego servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuro Swagger con soporte para JWT
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter a JWT token as: Bearer {token}",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", scheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { scheme, Array.Empty<string>() }
    });
});

// Configuro la conexión a la base de datos SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<JwtTokenService>();

// Configuro la autenticación JWT
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "";
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "";

if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException("Jwt:Secret is required.");
}

// Configuro la autenticación JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var payload = JsonSerializer.Serialize(new { code = StatusCodes.Status401Unauthorized, message = "Unauthorized" });
                await context.Response.WriteAsync(payload);
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                var payload = JsonSerializer.Serialize(new { code = StatusCodes.Status403Forbidden, message = "Forbidden" });
                await context.Response.WriteAsync(payload);
            },
            OnAuthenticationFailed = async context =>
            {
                context.NoResult();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var payload = JsonSerializer.Serialize(new { code = StatusCodes.Status401Unauthorized, message = "Invalid token" });
                await context.Response.WriteAsync(payload);
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              );
});

// Construyo la aplicación.
var app = builder.Build();

app.UseCors("Frontend");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Agrego el middleware de manejo de errores
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
