namespace TravelSuggest.Models;
public class Suggestion
{
    // public readonly object CityName;

    public int Id { get; set;} 
    public string? Title { get; set;}
    public string? Description { get; set;}
    public decimal? Price { get; set;}
    public int? Rating { get; set; }
    public DateTime Created_at { get; set; } = DateTime.Now;
    public int? UserId { get; set; }
    public int? DestinationId { get; set; }
    public UserPreview User { get; set; } 
    public Destination Destination { get; set; } 

    public class UserPreview
    {
        public int Id { get; set; }
        public string? UserName { get; set; } 
    }

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