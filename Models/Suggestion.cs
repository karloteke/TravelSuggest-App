namespace TravelSuggest.Models;
public class Suggestion
{
    public int Id { get; set;} 
    public string? Title { get; set;}
    public string? Description { get; set;}
    public decimal? Price { get; set;}
    public int? Rating { get; set; }
    public DateTime Created_at { get; set; } = DateTime.Now;
    public int? UserId { get; set; }
    public int? DestinationId { get; set; } 
    // public User User { get; set; }
    // public Destination Destination { get; set; } 

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