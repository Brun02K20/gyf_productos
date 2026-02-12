using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace backend.Dtos.Products;

public class DeleteProductDto
{
    [Required]
    [SwaggerSchema(Description = "ID del producto a eliminar")]
    [DefaultValue(1)] // Valor por defecto para Swagger, no afecta la validación real
    public int Id { get; set; }

    [Required]
    [SwaggerSchema(Description = "Status del producto a borrar")]
    [DefaultValue(200)] // Valor por defecto para Swagger, no afecta la validación real
    public int Status { get; set; } = 200;

    [Required]
    [SwaggerSchema(Description = "Mensaje de respuesta al eliminar el producto")]
    [DefaultValue("Producto eliminado exitosamente")] // Valor por defecto para Swagger, no afecta la validación real
    public string Message { get; set; } = string.Empty;
}