using System.ComponentModel.DataAnnotations;

namespace TravelSuggest.Models; 

public class SuggestionCreateDTO
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
    public decimal? Price { get; set; }

    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
    public int Rating { get; set; }
    
    [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor que cero.")]
    public int UserId { get; set; }
}

