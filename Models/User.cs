namespace TravelSuggest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }



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
        }

         public static void UpdateNextUserId(int nextId)
        {
            // NextUserId = nextId;
        }
    }
}
