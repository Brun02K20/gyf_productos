using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace backend.Dtos.Products;

public class ProductDto
{
    [SwaggerSchema(Description = "ID del producto")]
    [DefaultValue(1)] // Valor por defecto para Swagger, no afecta la validación real
    public int Id { get; set; }

    [Required]
    [SwaggerSchema(Description = "Precio del producto")]
    [DefaultValue("99.00")] // Valor por defecto para Swagger, no afecta la validación real
    public string Price { get; set; } = string.Empty;

    [Required]
    [SwaggerSchema(Description = "Fecha de creación del producto")]
    [DefaultValue("2024-01-01 12:00:00")] // Valor por defecto para Swagger, no afecta la validación real
    public string CreatedAt { get; set; } = string.Empty;

    [Required]
    [SwaggerSchema(Description = "Nombre de la categoría del producto")]
    [DefaultValue("Limpieza")] // Valor por defecto para Swagger, no afecta la validación real
    public string CategoryName { get; set; } = string.Empty;
}