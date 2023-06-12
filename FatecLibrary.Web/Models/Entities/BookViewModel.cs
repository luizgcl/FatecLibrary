using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FatecLibrary.Web.Models.Entities;
public class BookViewModel 
{
    public int Id { get; set; }
    [Required]
    [DisplayName("Titulo")]
    public string? Title { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}")]
    [DisplayName("Preço")]
    [Required]
    public decimal Price { get; set; }
    [DisplayName("Ano de Publicação")]
    [Required]
    public int PublicationYear { get; set; }
    [DisplayName("N de Edição")]
    [Required]
    public int Edition { get; set; }
    
    public string? ImageURL { get; set; }
    public string? PublishingName { get; set; }
    [Required]
    [Display(Name = "Publishers")]
    public int PublishingId { get; set; }
}
