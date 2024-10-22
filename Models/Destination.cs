    namespace TravelSuggest.Models;
    public class Destination
    {
        public int Id { get; set;} 
        public string? CityName { get; set;}
        public string? Description { get; set;}
        public string? Season { get; set;}
        public bool IsPopular { get; set; }
        public string? Category { get; set; }
        public int? UserId { get; set; }

        // Constructor sin parámetros
        public Destination() { }


    public Destination ( string cityName, string description, string season, bool isPopular, string category, int userId)
    {
        CityName = cityName;
        Description = description;
        Season = season;
        IsPopular = isPopular;
        Category = category;
        UserId = userId;
    }
}