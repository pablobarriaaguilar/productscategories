#pragma warning disable CS8618
// We can disable our warnings safely because we know the framework will assign non-null values 
// when it constructs this class for us.
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace productscategories.Models;

public class Categorie{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List <Asssociation> Asssociations { get; set; } = new List<Asssociation> ();
}