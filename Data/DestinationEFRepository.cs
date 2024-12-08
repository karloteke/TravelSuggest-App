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
            _context.Destinations.Add(destination);
            SaveChanges();
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

            // Ordena por ID descendente para que el destino más reciente aparezca primero
            query = query.OrderByDescending(d => d.Id);

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
                    _context.Destinations.Remove(destination);
                    SaveChanges();
                }
            }
        }

        public void UpdateDestination(Destination destination, int userId)
        {
            _context.Entry(destination).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
