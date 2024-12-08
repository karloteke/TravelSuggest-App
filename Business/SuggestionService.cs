using TravelSuggest.Models;
using TravelSuggest.Data;

namespace TravelSuggest.Business
{
    public class SuggestionService : ISuggestionService
    {
        private readonly ISuggestionRepository _repository;

        public SuggestionService(ISuggestionRepository repository)
        {
            _repository = repository;
        }


        public void CreateSuggestion(string title, string description, decimal price, int rating, DateTime created_at, int destinationId, int userId)
        {
            // Verificar si el destino existe
            var destination = _repository.GetDestinationById(destinationId);

            // Si el destino no existe, lanzamos una excepción
            if (destination == null)
            {
                throw new InvalidOperationException($"El destino con ID {destinationId} no existe.");

            }

            // Verificar si el usuario existe
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"El usuario con ID {userId} no existe.");
            }

            // Crear la sugerencia ya que el destino existe
            var suggestion = new Suggestion(title, description, price, rating, created_at, userId, destinationId);

            _repository.AddSuggestion(suggestion, userId);  // Agregar la sugerencia al repositorio

            user.AddPoints(50);  // Asignar puntos al usuario
            
            _repository.SaveChanges(); 
        }


        public List<Suggestion> GetAllSuggestions()
        {
            return _repository.GetAllSuggestions();
        }

        public IEnumerable<Suggestion> GetAllSuggestions(SuggestionQueryParameters? SuggestionQueryParameters)
        {
            return _repository.GetAllSuggestions(SuggestionQueryParameters);
        }

        public Destination? GetDestinationById(int destinationId)
        {
            var Destination = _repository.GetDestinationById(destinationId);

            if(Destination == null)
            {
                throw new KeyNotFoundException($"El destino con Id {destinationId} no existe.");
            }
            return Destination;
        }

        public Suggestion GetSuggestionById(int SuggestionId)
        {
            var Suggestion = _repository.GetSuggestionById(SuggestionId);
            
            if(Suggestion == null)
            {
                  throw new KeyNotFoundException($"La sugerencia con Id {SuggestionId} no existe.");
            }
            return Suggestion;
        }

        public List<Suggestion> GetSuggestions(int destinationId)
        {
           // Obtener todas los destinos asociados a un usuario por su ID
            var Suggestions = _repository.GetSuggestions(destinationId);

            if (Suggestions == null)
            {
                throw new KeyNotFoundException($"No hay sugerencias para el destino con ID: {destinationId}");
            }
            return Suggestions;
        }

        public void UpdateSuggestionDetails(int suggestionId, SuggestionUpdateDTO suggestionUpdate)
        {
            var suggestion = _repository.GetSuggestionById(suggestionId);

            if (suggestion == null)
            {
                throw new KeyNotFoundException($"La sugerencia con id: {suggestionId} no existe.");
            }

            suggestion.Title = suggestionUpdate.Title;
            suggestion.Description = suggestionUpdate.Description;
            suggestion.Price = suggestionUpdate.Price;
            suggestion.Rating = suggestionUpdate.Rating;
            _repository.UpdateSuggestion(suggestion, suggestionId );
            _repository.SaveChanges();
        }


        public void DeleteSuggestion(int suggestionId)
            {
                // Obtener la sugerencia
                var suggestion = _repository.GetSuggestionById(suggestionId);
                if (suggestion == null)
                {
                    throw new KeyNotFoundException($"El destino con Id: {suggestionId} no existe.");
                }

                // Obtener el usuario asociado
                var user = _repository.GetUserById(suggestion.UserId);
                if (user != null)
                {
                    user.DeductPoints(50); // Restamos 50 puntos por eliminar la sugerencia
                }

                _repository.DeleteSuggestion(suggestionId);

                _repository.SaveChanges();
            }
    }
}