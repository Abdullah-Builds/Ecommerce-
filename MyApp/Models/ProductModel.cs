using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models;

public class ProductModel
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string Name { get; set; } = default!;

    [StringLength(70)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [StringLength(20)]
    public string? Tag { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MutedPrice { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    [StringLength(20)]
    public string? Category { get; set; }

    [StringLength(20)]
    public string? Color { get; set; }

    [StringLength(20)]
    public string? Size { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public int Stock { get; set; } = 0;

    public bool IsAvailable { get; set; } = true;

}
