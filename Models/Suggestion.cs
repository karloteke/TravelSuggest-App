namespace TravelSuggest.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Suggestion
{
    [Key]
    public int Id { get; set;}
    [Required] 
    public string Title { get; set;} = string.Empty;
    [Required] 
    public string Description { get; set;} = string.Empty;
    [Required] 
    public decimal? Price { get; set;}
    [Required] 
    public int? Rating { get; set; }
    public DateTime Created_at { get; set; } = DateTime.Now;
    
    [ForeignKey("User")]
    public int? UserId { get; set; }
    [ForeignKey("Destination")]
    public int? DestinationId { get; set; }
    
    public UserPreviewDTO? User { get; set; }
    public Destination Destination { get; set; } 

    // Constructor sin parámetros
    public Suggestion() { }


    public Suggestion ( string title, string description, decimal price, int rating, DateTime created_at, int userId, int destinationId)
    {
        Title = title;
        Description = description;
        Price = price;
        Rating = rating;
        Created_at = created_at;
        UserId = userId;
        DestinationId = destinationId;
    }
}