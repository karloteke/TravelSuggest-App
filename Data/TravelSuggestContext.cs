using TravelSuggest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

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
            base.OnModelCreating(modelBuilder);

            // Agregar usuarios (2 Administradores y 4 Usuarios normales)
            modelBuilder.Entity<User>().HasData(
                // Administradores
                new User
                {
                    Id = 1,
                    UserName = "Carlota",
                    Password = "carlota37",
                    Email = "carlota@gmail.com",
                    Points = 0,
                    Role = Roles.Admin
                },
                new User
                {
                    Id = 2,
                    UserName = "Jesús",
                    Password = "jesus31",
                    Email = "jesus@gmail.com",
                    Points = 0,
                    Role = Roles.Admin
                },

                // Usuarios normales
                 new User
                {
                    Id = 3,
                    UserName = "Jumar",
                    Password = "jumar123",
                    Email = "jumar@gmail.com",
                    Points = 300, // 100 (registro) + 150 (destino) + 50 (sugerencia)
                    Role = Roles.User
                },
                new User
                {
                    Id = 4,
                    UserName = "Ana",
                    Password = "ana123",
                    Email = "ana@gmail.com",
                    Points = 300,
                    Role = Roles.User
                },
                new User
                {
                    Id = 5,
                    UserName = "Nerea",
                    Password = "nerea123",
                    Email = "nerea@gmail.com",
                    Points = 300,
                    Role = Roles.User
                },
                new User
                {
                    Id = 6,
                    UserName = "Bea",
                    Password = "bea123",
                    Email = "bea@gmail.com",
                    Points = 300,
                    Role = Roles.User
                },
                new User
                {
                    Id = 7,
                    UserName = "Laura",
                    Password = "laura123",
                    Email = "laura@gmail.com",
                    Points = 300,
                    Role = Roles.User
                },
                new User
                {
                    Id = 8,
                    UserName = "Pilar",
                    Password = "pilar123",
                    Email = "pilar@gmail.com",
                    Points = 300,
                    Role = Roles.User
                }
            );

            modelBuilder.Entity<Destination>().HasData(
                  new Destination
                {
                    Id = 1,
                    CityName = "Barcelona",
                    Description = "Barcelona combina playas, arquitectura modernista y una vibrante vida nocturna.",
                    Season = "Verano",
                    IsPopular = true,
                    Category = "Playa",
                    UserId = 3 // Jumar
                },
                new Destination
                {
                    Id = 2,
                    CityName = "Madrid",
                    Description = "Madrid es conocida por sus museos de renombre, parques expansivos y una animada escena gastronómica.",
                    Season = "Invierno",
                    IsPopular = true,
                    Category = "Gastronomía",
                    UserId = 4 // Ana
                },
                new Destination
                {
                    Id = 3,
                    CityName = "Granada",
                    Description = "Granada alberga la impresionante Alhambra y una rica mezcla de culturas.",
                    Season = "Primavera",
                    IsPopular = true,
                    Category = "Cultural",
                    UserId = 5 // Nerea
                },
                new Destination
                {
                    Id = 4,
                    CityName = "Londres",
                    Description = "Explora la aventura urbana y los emocionantes parques de Londres con actividades para todos.",
                    Season = "Otoño",
                    IsPopular = false,
                    Category = "Aventura",
                    UserId = 6 // Bea
                },
                 new Destination
                {
                    Id = 5,
                    CityName = "Palma de Mallorca",
                    Description = "Conocida por sus impresionantes calas, playas y una vibrante vida nocturna.",
                    Season = "Todas las estaciones",
                    IsPopular = true,
                    Category = "Ocio",
                    UserId = 7 // Laura
                },
                new Destination
                {
                    Id = 6,
                    CityName = "Ámsterdam",
                    Description = "Ámsterdam es famosa por sus canales, museos y una animada vida cultural.",
                    Season = "Primavera",
                    IsPopular = true,
                    Category = "Ciudad",
                    UserId = 8 // Pilar
                }
            );

            modelBuilder.Entity<Suggestion>().HasData(
                new Suggestion
                {
                    Id = 1,
                    Title = "Clases de surf en la Barceloneta",
                    Description = "Aprende a surfear en las famosas playas de Barcelona con instructores profesionales.",
                    Price = 60.00m,
                    Rating = 5,
                    Created_at = DateTime.Now,
                    UserId = 3, // Jumar
                    DestinationId = 1 // Barcelona
                },
                new Suggestion
                {
                    Id = 2,
                    Title = "Tour gastronómico por el Mercado de San Miguel",
                    Description = "Degusta una variedad de tapas y especialidades locales en el histórico Mercado de San Miguel.",
                    Price = 50.00m,
                    Rating = 5,
                    Created_at = DateTime.Now,
                    UserId = 4, // Ana
                    DestinationId = 2 // Madrid
                },
                new Suggestion
                {
                    Id = 3,
                    Title = "Visita guiada a la Alhambra",
                    Description = "Explora los majestuosos palacios y jardines de la Alhambra con un guía experto.",
                    Price = 30.00m,
                    Rating = 5,
                    Created_at = DateTime.Now,
                    UserId = 5, // Nerea
                    DestinationId = 3 // Granada
                },
                new Suggestion
                {
                    Id = 4,
                    Title = "Senderismo en Hampstead Heath",
                    Description = "Disfruta de impresionantes vistas de Londres mientras recorres las rutas de Hampstead Heath.",
                    Price = 25.00m,
                    Rating = 4,
                    Created_at = DateTime.Now,
                    UserId = 6, // Bea
                    DestinationId = 4 // Londres
                },
                 new Suggestion
                {
                    Id = 5,
                    Title = "Paseo en barco por las calas de Mallorca",
                    Description = "Descubre calas escondidas y disfruta de aguas cristalinas en un paseo en barco.",
                    Price = 150.00m,
                    Rating = 5,
                    Created_at = DateTime.Now,
                    UserId = 7, // Laura
                    DestinationId = 5 // Palma de Mallorca
                },
                new Suggestion
                {
                    Id = 6,
                    Title = "Tour en bicicleta por los canales",
                    Description = "Explora Ámsterdam como un local en un tour en bicicleta por sus canales históricos.",
                    Price = 40.00m,
                    Rating = 4,
                    Created_at = DateTime.Now,
                    UserId = 8, // Pilar
                    DestinationId = 6 // Ámsterdam
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging(); // Opcional para desarrollo
        }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
