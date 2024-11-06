using TravelSuggest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TravelSuggest.Data
{
    public class TravelSuggestContext : DbContext
    {
        public TravelSuggestContext(DbContextOptions<TravelSuggestContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llamamos al método base para asegurarnos de que se ejecute cualquier lógica definida allí.
            base.OnModelCreating(modelBuilder);

            // Agregar usuarios primero
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "Carlota", Password = "Carlota36", Email = "carlota@gmail.com", Points = 0, Role = Roles.Admin },
                new User { Id = 2, UserName = "Ana", Password = "Ana123", Email = "ana@gmail.com", Points = 300, Role = Roles.User }, // Registro
                new User { Id = 3, UserName = "Jesus", Password = "Jesus30", Email = "jesus@gmail.com", Points = 300, Role = Roles.User }, // Registro
                new User { Id = 4, UserName = "Paola", Password = "Paola123", Email = "paola@gmail.com", Points = 300, Role = Roles.User }, // Registro
                new User { Id = 5, UserName = "Pilar", Password = "Pilar123", Email = "pilar@gmail.com", Points = 300, Role = Roles.User }  // Registro
            );

            // Luego agregar destinos con UserId
            modelBuilder.Entity<Destination>().HasData(
                new Destination { Id = 1, CityName = "Londres", Description = "Explora la rica historia, cultura y entretenimiento que ofrece Londres, desde museos gratuitos hasta vibrantes mercados y parques.", Season = "Todo el año", IsPopular = false, Category = "Ocio", UserId = 2 }, // Ana crea este destino
                new Destination { Id = 2, CityName = "París", Description = "La capital francesa es famosa por su exquisita gastronomía, desde panaderías hasta restaurantes de alta cocina.", Season = "Primavera", IsPopular = true, Category = "Gastronómica", UserId = 3 }, // Jesús crea este destino
                new Destination { Id = 3, CityName = "Ámsterdam", Description = "Ámsterdam es conocida por sus museos, arquitectura y vibrante escena artística.", Season = "Verano", IsPopular = true, Category = "Cultural", UserId = 4 }, // Paola crea este destino
                new Destination { Id = 4, CityName = "Mallorca", Description = "La isla de Mallorca ofrece una amplia variedad de actividades al aire libre, desde senderismo hasta deportes acuáticos.", Season = "Verano", IsPopular = true, Category = "Aventura", UserId = 5 } // Pilar crea este destino
            );

            // Finalmente agregar sugerencias con UserId
            modelBuilder.Entity<Suggestion>().HasData(
                // Sugerencias para Londres
                new Suggestion { Id = 1, Title = "Tour en barco por el río Támesis", Description = "Disfruta de una experiencia única viendo los principales monumentos de Londres desde el agua.", Price = 25.00m, Rating = 4, Created_at = DateTime.Now, UserId = 3, DestinationId = 1 }, // Jesús
                
                // Sugerencias para París
                new Suggestion { Id = 2, Title = "Tour gastronómico por Montmartre", Description = "Explora las delicias culinarias de París mientras paseas por uno de los barrios más emblemáticos de la ciudad.", Price = 60.00m, Rating = 4, Created_at = DateTime.Now, UserId = 5, DestinationId = 2 }, // Pilar
                
                // Sugerencias para Ámsterdam
                new Suggestion { Id = 3, Title = "Recorrido en bicicleta por los canales", Description = "Explora la ciudad sobre dos ruedas y descubre su belleza única.", Price = 15.00m, Rating = 4, Created_at = DateTime.Now, UserId = 2, DestinationId = 3 }, // Ana
                
                // Sugerencias para Mallorca
                new Suggestion { Id = 4, Title = "Tour motos de agua en Cala Gamba", Description = "Descubre calas escondidas y cuevas marinas en un tour de motos de agua guiado.", Price = 150.00m, Rating = 5, Created_at = DateTime.Now, UserId = 4, DestinationId = 4 } // Paola
            );
        }
                
                
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging(); // Opcional
        }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<User> Users { get; set; }
    }

}