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
    [DefaultValue("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyIiwibmFtZSI6IkJydW5vIiwiaWF0IjoxNjg4ODQyODkxfQ.2sH8n7l3")] // Valor por defecto para Swagger, no afecta la validación real
    public string Token { get; set; } = string.Empty;

    [SwaggerSchema(Description = "Nombre de usuario autenticado")]
    [DefaultValue("Bruno")] // Valor por defecto para Swagger, no afecta la validación real
    public string Username { get; set; } = string.Empty;
}

public class RegisterUser
{
    [SwaggerSchema(Description = "ID del usuario registrado")]
    [DefaultValue(1)] // Valor por defecto para Swagger, no afecta la validación real
    public int Id { get; set; } = 1;

    [Required]
    [SwaggerSchema(Description = "Nombre de usuario registrado")]
    [DefaultValue("Bruno")] // Valor por defecto para Swagger, no afecta la validación real
    public string Username { get; set; } = string.Empty;

    [Required]
    [SwaggerSchema(Description = "Contraseña para el usuario registrado")]
    [DefaultValue("2shasdlifjsdakjghskdfjghdfkjsghdjskflghdskfghdkjsfghdkfghdkfghdfkghdfkg")] // Valor por defecto para Swagger, no afecta la validación real
    public string Password { get; set; } = string.Empty;
}