using System.ComponentModel.DataAnnotations;

namespace TravelSuggest.Models; 

public class SuggestionUpdateDTO
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
    public decimal? Price { get; set; }

    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
    public int Rating { get; set; }  
}
