using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TravelSuggest.Models;

namespace TravelSuggest.Data
{
    public class DestinationEFRepository : IDestinationRepository
    {
        private readonly TravelSuggestContext _context;

        public DestinationEFRepository(TravelSuggestContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddDestination(Destination destination, int userId)
        {
            // Verificar si existe el destino en la base de datos
            if (!_context.Destinations.Any(d => d.Id == destination.Id))
            {
                _context.Destinations.Add(destination);
                SaveChanges();

                // Asignar puntos al usuario
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.AddPoints(150); // Asigna 150 puntos por crear un nuevo destino
                    SaveChanges(); 
                }
            }
        }

        public IEnumerable<Destination> GetAllDestinations(DestinationQueryParameters? destinationQueryParameters)
        {
            var query = _context.Destinations.AsQueryable();

            // Filtrar por nombre del destino
            if (!string.IsNullOrWhiteSpace(destinationQueryParameters?.CityName))
            {
                query = query.Where(d => d.CityName != null && d.CityName.Contains(destinationQueryParameters.CityName));
            }

            // Filtrar por estación del año
            if (!string.IsNullOrWhiteSpace(destinationQueryParameters?.Season))
            {
                query = query.Where(d => d.Season != null && d.Season.Contains(destinationQueryParameters.Season));
            }

            // Filtrar por categoria
            if (!string.IsNullOrWhiteSpace(destinationQueryParameters?.Category))
            {
                query = query.Where(d => d.Category != null && d.Category.Contains(destinationQueryParameters.Category));
            }

            // Filtrar por UserId
            if (destinationQueryParameters?.UserId.HasValue == true)
            {
                query = query.Where(d => d.UserId == destinationQueryParameters.UserId.Value);
            }

            // Filtrar por popularidad
            if (destinationQueryParameters?.IsPopular.HasValue == true)
            {
                query = query.Where(d => d.IsPopular == destinationQueryParameters.IsPopular.Value);
            }

            return query.ToList();
        }

        public List<Destination> GetAllDestinations()
        {
            return _context.Destinations.ToList();
        }

        public User? GetUserById(int? userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public Destination GetDestinationById(int destinationId)
        {
            return _context.Destinations.FirstOrDefault(d => d.Id == destinationId);
        }

        public List<Destination> GetDestinations(int? userId)
        {
            // Implementación para obtener todas los destinos asociados a un usuario por su Id
            return _context.Destinations.Where(du => du.UserId == userId).ToList();
        }

        public void DeleteDestination(int? destinationId)
        {
            if (destinationId != null)
            {
                var destination = _context.Destinations.FirstOrDefault(d => d.Id == destinationId);
                if (destination != null)
                {
                    // Restar puntos al usuario
                    var user = _context.Users.FirstOrDefault(u => u.Id == destination.UserId);
                    if (user != null)
                    {
                        user.DeductPoints(150); // Resta 150 puntos al eliminar el destino
                        SaveChanges(); 
                    }

                    _context.Destinations.Remove(destination);
                    SaveChanges();
                }
            }
        }

        public void UpdateDestination(Destination destination, int userId)
        {
            _context.Entry(destination).State = EntityState.Modified;

            // Opcional: Asignar puntos al usuario si es necesario al actualizar
            // var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            // if (user != null)
            // {
            //     user.AddPoints(150); // Asigna 150 puntos al actualizar un destino
            //     SaveChanges(); 
            // }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
