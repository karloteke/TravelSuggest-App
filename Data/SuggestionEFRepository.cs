using System.Linq;
using Microsoft.EntityFrameworkCore;
using TravelSuggest.Models;

namespace TravelSuggest.Data
{
    public class SuggestionEFRepository : ISuggestionRepository
    {
        private readonly TravelSuggestContext _context;
        private readonly IDestinationRepository _destinationRepository;
        private readonly IUserRepository _userRepository;

        public SuggestionEFRepository(
            TravelSuggestContext context,
            IDestinationRepository destinationRepository,
            IUserRepository userRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _destinationRepository = destinationRepository ?? throw new ArgumentNullException(nameof(destinationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public void AddSuggestion(Suggestion suggestion, int userId)
        {
            // Verifica que el DestinationId no sea null
            if (!suggestion.DestinationId.HasValue)
            {
                throw new InvalidOperationException("El DestinationId no puede ser nulo.");
            }

            // Verifica que el destino exista antes de continuar
            var destination = _destinationRepository.GetDestinationById(suggestion.DestinationId.Value);
            if (destination == null)
            {
                throw new InvalidOperationException("El destino no existe.");
            }

            // Verifica si una sugerencia similar ya existe para el mismo destino y usuario
            if (!_context.Suggestions.Any(s => s.DestinationId == suggestion.DestinationId.Value && s.UserId == userId))
            {
                suggestion.UserId = userId;
                _context.Suggestions.Add(suggestion);
                SaveChanges();

                var user = _userRepository.GetUserById(userId);
                if (user != null)
                {
                    user.AddPoints(50); // Sumamos 50 puntos al crear la sugerencia
                    _userRepository.SaveChanges();
                }
            }
        }

        public IEnumerable<Suggestion> GetAllSuggestions(SuggestionQueryParameters? suggestionQueryParameters)
        {
            var query = _context.Suggestions
            .Include(s => s.Destination) 
            .AsQueryable();

            // Filtrar por DestinationId
            if (suggestionQueryParameters?.DestinationId.HasValue == true)
            {
                query = query.Where(s => s.DestinationId == suggestionQueryParameters.DestinationId.Value);
            }

            // Filtrar por rango de precios
            if (suggestionQueryParameters?.MinPrice.HasValue == true && suggestionQueryParameters?.MaxPrice.HasValue == true)
            {
                query = query.Where(s => s.Price >= suggestionQueryParameters.MinPrice.Value && s.Price <= suggestionQueryParameters.MaxPrice.Value);
            }
            else if (suggestionQueryParameters?.MinPrice.HasValue == true)
            {
                query = query.Where(s => s.Price >= suggestionQueryParameters.MinPrice.Value);
            }
            else if (suggestionQueryParameters?.MaxPrice.HasValue == true)
            {
                query = query.Where(s => s.Price <= suggestionQueryParameters.MaxPrice.Value);
            }

            // Filtrar por puntuación
            if (suggestionQueryParameters?.Rating.HasValue == true)
            {
                query = query.Where(s => s.Rating == suggestionQueryParameters.Rating.Value);
            }

            // Ejecutar la consulta final y convertirla a lista
            var suggestions = query.ToList();

            // Agregar manualmente UserPreviewDTO a cada sugerencia
            foreach (var suggestion in suggestions)
            {
                var user = _userRepository.GetUserById(suggestion.UserId);
                if (user != null)
                {
                    suggestion.User = new UserPreviewDTO { Id = user.Id, UserName = user.UserName };
                }
            }

            return suggestions;
        }

        public List<Suggestion> GetAllSuggestions()
        {
            return _context.Suggestions.Include(s => s.User).Include(s => s.Destination).ToList();
        }

        public Destination? GetDestinationById(int? destinationId)
        {
            return _destinationRepository.GetDestinationById((int)destinationId);
        }

        public Suggestion GetSuggestionById(int? suggestionId)
        {
            return _context.Suggestions
                .Include(s => s.User)
                .Include(s => s.Destination)
                .FirstOrDefault(s => s.Id == suggestionId);
        }

        public List<Suggestion> GetSuggestions(int? destinationId)
        {
            return _context.Suggestions.Where(d => d.DestinationId == destinationId).ToList();
        }

        public void DeleteSuggestion(int? suggestionId)
        {
            if (suggestionId == null)
            {
                return;
            }

            var suggestion = _context.Suggestions.FirstOrDefault(s => s.Id == suggestionId);
            if (suggestion != null)
            {
                var user = _userRepository.GetUserById(suggestion.UserId);
                if (user != null)
                {
                    user.DeductPoints(50); // Resta 50 puntos al eliminar la sugerencia
                    _userRepository.SaveChanges();
                }

                _context.Suggestions.Remove(suggestion);
                SaveChanges();
            }
        }

        public void UpdateSuggestion(Suggestion suggestion, int userId)
        {
            if (suggestion.Id != 0)
            {
                _context.Entry(suggestion).State = EntityState.Modified;
                SaveChanges();
            }
            else
            {
                AddSuggestion(suggestion, userId);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
