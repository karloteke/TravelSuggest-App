namespace TravelSuggest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int Points { get; set; }

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
