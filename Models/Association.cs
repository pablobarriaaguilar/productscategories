#pragma warning disable CS8618
// We can disable our warnings safely because we know the framework will assign non-null values 
// when it constructs this class for us.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
namespace productscategories.Models;

public class Asssociation{
    [Key]
    public int AssociationId { get; set; }
    public int ProductId { get; set; }
    public int CategoryId {get; set; }
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [ForeignKey("CategoryId")]
    public Categorie? Categorie { get; set; }

    
}