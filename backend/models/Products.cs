using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Products")]
public class Products
{
    [Key]
    public int Id { get; set; }

    [Range(1, 1000000)]
    [Required]
    public decimal Price { get; set; }

    [ForeignKey("Categories")]
    [Required]
    [Column("id_category")]
    public int IdCategory { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}