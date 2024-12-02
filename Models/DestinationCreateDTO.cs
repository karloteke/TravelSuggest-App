using System.ComponentModel.DataAnnotations;

namespace TravelSuggest.Models; 

public class DestinationCreateDTO
{
    [Required(ErrorMessage = "La ciudad es obligatoria.")]
    public string? CityName { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "La estación del año es obligatoria.")]
    public string? Season { get; set; }

    public bool? IsPopular { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string? Category { get; set; }  

    public string? ImageBase64 { get; set; }

}
