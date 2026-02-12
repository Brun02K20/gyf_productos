using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace backend.Dtos.Products;

public class CreateProductDto
{
    [Required]
    [Range(1, 1000000)]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe tener máximo 2 decimales")]
    [SwaggerSchema(Description = "Precio del producto")]
    [DefaultValue(99.00)] // Valor por defecto para Swagger, no afecta la validación real
    public decimal Price { get; set; }

    [Required]
    [SwaggerSchema(Description = "ID de la categoría del producto")]
    [DefaultValue(1)] // Valor por defecto para Swagger, no afecta la validación real
    public int CategoryId { get; set; }
}