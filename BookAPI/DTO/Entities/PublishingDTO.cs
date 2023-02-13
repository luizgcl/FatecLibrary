using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookAPI.DTO.Entities;

public class PublishingDTO {

    public int Id { get; set; }

    [Required(ErrorMessage = "The Name is required!")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Name { get; set; }
    public string? Acronym { get; set; }
    public ICollection<BookDTO>? BooksDTO { get; set; }


}
