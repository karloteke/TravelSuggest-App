using System.ComponentModel.DataAnnotations;


namespace TravelSuggest.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        public int Points { get; set; }
        public string Role { get; set; }  = Roles.User;

         public void AddPoints(int points)
        {
            Points += points; // Sumar puntos al usuario
        }

        public void DeductPoints(int points)
        {
            Points = Math.Max(0, Points - points); // Restar puntos sin permitir que se vuelva negativo
        }

        private static int NextUserId = 1;

        // Constructor sin parámetros requerido para la deserialización
        public User() 
        { 
            //  Id = NextUserId++;
        }

        public User(string? userName, string? password, string? email)
        { 
            // Id = NextUserId++;
            UserName = userName;
            Password = password;
            Email = email;
            Points = 100;
        }

         public static void UpdateNextUserId(int nextId)
        {
            // NextUserId = nextId;
        }
    }
}
