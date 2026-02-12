// DTO con id y token para el login
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace backend.Dtos.Users;


public class UserCredentials
{
    [Required]
    [SwaggerSchema(Description = "Nombre de usuario para autenticación")]
    [DefaultValue("Bruno")] // Valor por defecto para Swagger, no afecta la validación real
    public string Username { get; set; } = string.Empty;

    [Required]
    [SwaggerSchema(Description = "Contraseña para autenticación")]
    [DefaultValue("Bruno")] // Valor por defecto para Swagger, no afecta la validación real
    public string Password { get; set; } = string.Empty;
}

public class LoginUser
{
    [SwaggerSchema(Description = "ID del usuario autenticado")]
    [DefaultValue(2)] // Valor por defecto para Swagger, no afecta la validación real
    public int Id { get; set; }
    [SwaggerSchema(Description = "Token de usuario autenticado")]
    public string Token { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Nombre de usuario autenticado")]
    [DefaultValue("Bruno")] // Valor por defecto para Swagger, no afecta la validación real
    public string Username { get; set; } = string.Empty;
}

public class RegisterUser
{
    public int Id { get; set; } = 0;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}