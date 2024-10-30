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

            // Crear la sugerencia ya que el destino existe
            var suggestion = new Suggestion(title, description, price, rating, created_at, userId, destinationId);

            // Agregar la sugerencia al repositorio
            _repository.AddSuggestion(suggestion, userId);
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

        public void DeleteSuggestion(int SuggestionId)
        {
            var User = _repository.GetSuggestionById(SuggestionId);

            if (User == null)
            {
                throw new KeyNotFoundException($"La sugerencia con Id: {SuggestionId} no existe.");
            }
             _repository.DeleteSuggestion(SuggestionId);         
        }
    }
}