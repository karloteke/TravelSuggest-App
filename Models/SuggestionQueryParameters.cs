using System.ComponentModel.DataAnnotations;

namespace TravelSuggest.Models;

public class SuggestionQueryParameters
{
     public int? DestinationId { get; set; }
    public decimal? MinPrice { get; set; }  // Precio mínimo
    public decimal? MaxPrice { get; set; }  // Precio máximo
    public int? Rating { get; set; }
}

