using System.ComponentModel.DataAnnotations;

namespace TravelSuggest.Models;

public class DestinationQueryParameters
{
    public string? CityName { get; set; }

    public string? Season { get; set; }

    public int? UserId { get; set;}

}

