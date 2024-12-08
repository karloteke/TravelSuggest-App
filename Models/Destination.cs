    namespace TravelSuggest.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Destination
    {
        [Key]
        public int Id { get; set;} 
        [Required] 
        public string CityName { get; set;} = string.Empty;
        [Required] 
        public string Description { get; set;} = string.Empty;
        [Required] 
        public string Season { get; set;} = string.Empty;
        [Required] 
        public bool IsPopular { get; set; }
        [Required] 
        public string Category { get; set; } = string.Empty;
        
        [ForeignKey("User")]
        public int? UserId { get; set; }

        public string? ImageBase64 { get; set; }

        // Constructor sin parámetros
        public Destination() { }


        public Destination ( string cityName, string description, string season, bool isPopular, string category, int userId, string imageBase64)
        {
            CityName = cityName;
            Description = description;
            Season = season;
            IsPopular = isPopular;
            Category = category;
            UserId = userId;
            ImageBase64 = imageBase64;
        }
    }